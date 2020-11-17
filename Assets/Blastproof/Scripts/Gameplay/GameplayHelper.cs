using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.Systems.UI;
using Blastproof.Utility;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class OccupiedSlotModel
{
    public string playerId;
    public int ballNumber;
}

[Serializable]
public class ActiveMatchModel
{
    public string id;
    public OccupiedSlotModel[] occupiedSlots;
    public string status;
    public int expiration;
    public int totalPrizePool;
    public int playerBall;
    public int minimumPlayersRequired;
    public int winnerBallNumber;
}

public class GameplayHelper : SerializedMonoBehaviour
{
    [BoxGroup("Data"), SerializeField] private IntEvent OtherPlayersChoseBallEvent;
    [BoxGroup("Data"), SerializeField] private IntEvent OtherPlayersDiscardedBallEvent;
    [BoxGroup("Data"), SerializeField] private IntEvent PlayerChoseBallEvent;
    [BoxGroup("Data"), SerializeField] private IntEvent PlayerChoseBallConfirmationEvent;
    [BoxGroup("Data"), SerializeField] private StringEvent PlayerChoseBallDenyEvent;
    [BoxGroup("Data"), SerializeField] private SimpleEvent PlayerDiscardedBallEvent;
    [BoxGroup("Data"), SerializeField] private IntVariable _pickedBall;
    [BoxGroup("Data"), SerializeField] private IntVariable _winnerBall;
    [BoxGroup("Data"), SerializeField] private IntVariable _requiredPlayersVariable;
    [BoxGroup("Data"), SerializeField] private IntVariable _playersVariable;
    [BoxGroup("Data"), SerializeField] private IntEvent ServerStartClock;
    [BoxGroup("Data"), SerializeField] private IntEvent ServerEndClock;


    [BoxGroup("UI"), SerializeField] private UISystem UISystemInstance;

    [BoxGroup("UI"), SerializeField] private UIState GameplayUIState;
    [BoxGroup("UI"), SerializeField] private UIState PlayerWinUISubState;
    [BoxGroup("UI"), SerializeField] private UIState PlayerLoseUISubState;
    [BoxGroup("UI"), SerializeField] private UIState PlayerParticipantUISubState;
    [BoxGroup("UI"), SerializeField] private IntVariable _prizePoolVariable;


    [DllImport("__Internal")]
    private static extern void BetOnBallExternal(int BallNumber, string MatchId);

    [DllImport("__Internal")]
    private static extern void CancelBetOnBallExternal(string MatchId);

    [DllImport("__Internal")]
    private static extern void RequestNewMatchExternal();

    private ActiveMatchModel activeMatchInfo;

    bool waitingForPlayerChoseBallResponse = false;
    
    private void OnEnable()
    {
        //_pickedBall.onValueChanged += OnPickedBallChanged;
        PlayerChoseBallEvent.Subscribe(OnPlayerChoseBall);
        PlayerDiscardedBallEvent.Subscribe(OnPlayerDiscardedBall);
        UISystemInstance.onStateChanged += UIStateChanged;
    }

    private void OnDisable()
    {
        PlayerChoseBallEvent.Unsubscribe(OnPlayerChoseBall);
        UISystemInstance.onStateChanged -= UIStateChanged;
        PlayerDiscardedBallEvent.Unsubscribe(OnPlayerDiscardedBall);
    }

    private void OnPlayerChoseBall(int Number)
    {
        //deliberate user action
        Debug.Log($"Picked Ball changed {Number}");

        waitingForPlayerChoseBallResponse = true;

#if UNITY_WEBGL && !UNITY_EDITOR
        BetOnBallExternal(Number, activeMatchInfo.id);
#endif
    }

    private void OnPlayerDiscardedBall()
    {
        //deliberate user action
        Debug.Log($"Player discarded bet");

        waitingForPlayerChoseBallResponse = true;
#if UNITY_WEBGL && !UNITY_EDITOR
        CancelBetOnBallExternal(activeMatchInfo.id);
#endif
    }

    public void UIStateChanged(UIState UIStateValue)
    {
        if(UIStateValue == GameplayUIState)
        {
            SetupMatch();

            if (activeMatchInfo?.playerBall > 0)
            {
                MEC.Timing.CallDelayed(0.1f, () => {
                    if(activeMatchInfo.playerBall != _pickedBall.Value && !waitingForPlayerChoseBallResponse)
                        _pickedBall.Value = activeMatchInfo.playerBall; 
                });
            }
        }
    }

    public void ReceiveActiveMatchInfo(string ActiveMatchInfo)
    {
        List<OccupiedSlotModel> previousSlots = new List<OccupiedSlotModel>();
        if (activeMatchInfo != null)
        {
            previousSlots = activeMatchInfo.occupiedSlots.ToList();
        }


        activeMatchInfo = JsonUtility.FromJson<ActiveMatchModel>(ActiveMatchInfo);
        //reset balls
        OccupiedSlotModel[] toResetSlots = previousSlots.Except(activeMatchInfo.occupiedSlots).ToArray();
        toResetSlots.ForEach(x =>
        {
            OtherPlayersDiscardedBallEvent?.Invoke(x.ballNumber);
        });

        SetupMatch();
    }

    private void HandleReceivePlayerBallFromServer()
    {
        if(!waitingForPlayerChoseBallResponse)
        {
            // if we're not waiting for a response from the API, but can update the _pickedBall safely
            // this case is very rare
            int _matchPickedBallValue = Mathf.Clamp(activeMatchInfo.playerBall, 0, activeMatchInfo.playerBall);
            if (_pickedBall.Value != _matchPickedBallValue)
                _pickedBall.Value = _matchPickedBallValue;
        }
        else
        {
            // if we're waiting for response but the server sends an update in which an occupied slot has user picked ball
            // there's no reason to wait for the response and just reset the ball with message for the user that the ball has been selected by another player
            if(activeMatchInfo.occupiedSlots.Where(x=>x.ballNumber == _pickedBall.Value).Count() > 0)
            {
                waitingForPlayerChoseBallResponse = false;
                _pickedBall.Value = 0;
            }
            //otherwise wait for confirmation
        }
        
    }

    public void PlayerChoseBallResponse(int value)
    {
        if(value == 0)
        {
            _pickedBall.Value = 0;
        }

        waitingForPlayerChoseBallResponse = false;
    }

    public void PlayerDiscardedBallResponse(int value)
    {
        if (value == 1)
        {
            _pickedBall.Value = 0;
        }

        waitingForPlayerChoseBallResponse = false;
    }

    private void SetupMatch()
    {
        if (activeMatchInfo == null)
            return;
        _requiredPlayersVariable.Value = activeMatchInfo.minimumPlayersRequired;
        switch (activeMatchInfo.status)
        {
            case "WAITING_FOR_PLAYERS":
                _playersVariable.Value = activeMatchInfo.occupiedSlots.Length + ((activeMatchInfo.playerBall > 0) ? 1 : 0);
                break;
            case "IN_PROGRESS":
                int remainingSeconds = activeMatchInfo.expiration - (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                ServerStartClock?.Invoke(remainingSeconds);
                break;
            case "ENDED":
                //show substate only if playing
                //otherwise it's just a spectator

                if(activeMatchInfo.playerBall > 0)
                {
                    UISystemInstance.ChangeState(activeMatchInfo.playerBall == activeMatchInfo.winnerBallNumber ? PlayerWinUISubState : PlayerLoseUISubState);
                }
                else
                {
                    //RequestNewMatch();
                    UISystemInstance.ChangeState(PlayerParticipantUISubState);
                }
                _winnerBall.Value = activeMatchInfo.winnerBallNumber;
                break;
            default:
                break;
        }

        HandleReceivePlayerBallFromServer();

        _prizePoolVariable.Value = activeMatchInfo.totalPrizePool;
        Debug.Log("Setup match ");
        activeMatchInfo.occupiedSlots.ForEach(x =>
        {
            OtherPlayersChoseBallEvent?.Invoke(x.ballNumber);
        });

    }

    public void RequestNewMatch()
    {
        Debug.Log("Requesting New Match");
        ResetActiveMatchInfo();
#if UNITY_WEBGL && !UNITY_EDITOR
        RequestNewMatchExternal();
#endif
    }

    [Button]
    public void ResetActiveMatchInfo()
    {
        Debug.Log("Reseting match");
        _playersVariable.Value = 0;
        if (activeMatchInfo != null)
        {
            activeMatchInfo.occupiedSlots.ForEach(x =>
            {
                OtherPlayersDiscardedBallEvent?.Invoke(x.ballNumber);
            });
        }
        _winnerBall.Value = 0;
        _pickedBall.Value = 0;
        _prizePoolVariable.Value = 0;
        activeMatchInfo = null;
        UISystemInstance.ChangeState(GameplayUIState);
        //UISystemInstance. PlayerWinUISubState
    }

    [Button]
    void SimulateMatchInfoReceive(ActiveMatchModel matchModel)
    {
        ReceiveActiveMatchInfo(JsonUtility.ToJson(matchModel));
    }

    [Button]
    void SimulatePlayerPickAccepted()
    {
        ActiveMatchModel testModel = new ActiveMatchModel();
        testModel.status = "WAITING_FOR_PLAYERS";
        testModel.minimumPlayersRequired = 10;
        testModel.playerBall = -1;
        testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 } };

        _pickedBall.Value = 4;
        OnPlayerChoseBall(4);

        MEC.Timing.CallDelayed(2f, () => {
            Debug.Log("Match without playerBall");
            SimulateMatchInfoReceive(testModel); 
        });

        MEC.Timing.CallDelayed(4f, () => {
            Debug.Log("Match without playerBall and new occupied slots");
            testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 }, new OccupiedSlotModel() { ballNumber = 2 } };
            SimulateMatchInfoReceive(testModel);
        });

        MEC.Timing.CallDelayed(5f, () => {
            Debug.Log("Server confirmation");
            PlayerChoseBallResponse(1); 
        }); 

        MEC.Timing.CallDelayed(6f, () => {
            Debug.Log("Match update with playerBall set");
            testModel.playerBall = 4;
            SimulateMatchInfoReceive(testModel); 
        });
    }

    [Button]
    void SimulatePlayerPickRejected()
    {
        ActiveMatchModel testModel = new ActiveMatchModel();
        testModel.status = "WAITING_FOR_PLAYERS";
        testModel.minimumPlayersRequired = 10;
        testModel.playerBall = -1;
        testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 } };

        _pickedBall.Value = 4;
        OnPlayerChoseBall(4);

        MEC.Timing.CallDelayed(2f, () => {
            Debug.Log("Match without playerBall");
            SimulateMatchInfoReceive(testModel);
        });

        MEC.Timing.CallDelayed(4f, () => {
            Debug.Log("Match without playerBall and new occupied slots");
            testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 }, new OccupiedSlotModel() { ballNumber = 2 } };
            SimulateMatchInfoReceive(testModel);
        });

        MEC.Timing.CallDelayed(6f, () => {
            Debug.Log("Match update with playerBall set");
            //testModel.playerBall = 4;
            testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 }, new OccupiedSlotModel() { ballNumber = 2 }, new OccupiedSlotModel() { ballNumber = 4 } };
            SimulateMatchInfoReceive(testModel);
        });
    }

    [Button]
    void SimulatePlayerDiscardedAccepted()
    {
        ActiveMatchModel testModel = new ActiveMatchModel();
        testModel.status = "WAITING_FOR_PLAYERS";
        testModel.minimumPlayersRequired = 10;
        testModel.playerBall = 4;
        testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 } };

        SimulateMatchInfoReceive(testModel);

        MEC.Timing.CallDelayed(2f, () => {
            _pickedBall.Value = 0;
            OnPlayerDiscardedBall();
        });

        MEC.Timing.CallDelayed(4f, () => {
            Debug.Log("Match without playerBall and new occupied slots");
            testModel.playerBall = 4;
            testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 }, new OccupiedSlotModel() { ballNumber = 2 } };
            SimulateMatchInfoReceive(testModel);
        });

        MEC.Timing.CallDelayed(5f, () =>
        {
            Debug.Log("Server confirmation");
            PlayerDiscardedBallResponse(1);
        });

        MEC.Timing.CallDelayed(6f, () => {
            Debug.Log("Match update with playerBall set");
            testModel.playerBall = -1;
            testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 }, new OccupiedSlotModel() { ballNumber = 2 }, new OccupiedSlotModel() { ballNumber = 4 } };
            SimulateMatchInfoReceive(testModel);
        });
    }

    [Button]
    void SimulatePlayerWinRestartGame()
    {
        ResetActiveMatchInfo();
        ActiveMatchModel testModel = new ActiveMatchModel();
        testModel.status = "ENDED";
        testModel.minimumPlayersRequired = 2;
        testModel.playerBall = 4;
        testModel.winnerBallNumber = 4;
        testModel.occupiedSlots = new OccupiedSlotModel[] { new OccupiedSlotModel() { ballNumber = 1 } };

        //SimulateMatchInfoReceive(testModel);

        MEC.Timing.CallDelayed(2f, () => {
            SimulateMatchInfoReceive(testModel);
        });
    }

}
