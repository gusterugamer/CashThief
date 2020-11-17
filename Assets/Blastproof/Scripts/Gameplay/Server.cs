using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JogaJoga
{
    /*
        This class is used to:
            * send events to the server
            * receive and handle them throughout the game
        Ideally, we should be using Events and Variables (which we should transfer to Obscured counterparts later on
    */
    [CreateAssetMenu(menuName = "Gameplay/Server")]
    public class Server : BlastproofSystem
    {
        [BoxGroup("Variables"), SerializeField] private IntVariable _pickedBall;

        [BoxGroup("Events"), SerializeField] private IntEvent _server_PlayerChoseBallConfirmation;
        [BoxGroup("Events"), SerializeField] private StringEvent _server_PlayerChoseBallDeny;
        [BoxGroup("Events"), SerializeField] private IntEvent _server_OtherPlayersChoseBall;
        [BoxGroup("Events"), SerializeField] private IntEvent _server_WinnerBall;
        [BoxGroup("Events"), SerializeField] private SimpleEvent _server_Reset;

        public override void Initialize()
        {
            base.Initialize();
            Reset();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _pickedBall.onValueChanged += Client_OnBallChosen;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _pickedBall.onValueChanged -= Client_OnBallChosen;
        }

        // ----- Client to server events
        public void Client_OnBallChosen()
        {
            // Let server know a ball's been chosen
            // Use _pickedBall.Value
        }

        // ---- Server to client events
        [Button]
        private void Server_Reset()
        {
            _pickedBall.Value = 0;
            _server_Reset.Invoke();
        }

        [Button]
        private void Server_BallChosenConfirmationReceived(int ballNr)
        {
            _server_PlayerChoseBallConfirmation.Invoke(ballNr);
        }

        [Button]
        private void Server_BallChosenConfirmationReceived(string error)
        {
            _server_PlayerChoseBallDeny.Invoke(error);
        }

        [Button]
        private void Server_OtherPlayersChoseBall(int ballNr)
        {
            _server_OtherPlayersChoseBall.Invoke(ballNr);
        }

        [Button]
        private void Server_WinnerBallReceived(int ballNr)
        {
            _server_WinnerBall.Invoke(ballNr);
        }
    }
}