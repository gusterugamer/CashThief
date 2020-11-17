using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private IntEvent _server_StartClock;
    [SerializeField] private IntEvent _server_EndClock;

    [SerializeField] private IntVariable _playersVariable;
    [SerializeField] private IntVariable _requiredPlayersVariable;

    [SerializeField] private TMPro.TextMeshProUGUI _timeText;
    [SerializeField] private TMPro.TextMeshProUGUI _playersText;

    float _startTime;

    //const int REQUIRED_PLAYERS = 3;
    //const int TIMER_SECONDS = 10;
    const string COROUTINE_TAG = "routine-Clock";

    private void OnEnable()
    {
        _server_StartClock.Subscribe(OnClockStarted);
        _server_EndClock.Subscribe(OnClockEnded);

        //_requiredPlayersVariable.onValueChanged += CheckRequiredPlayers;
        _playersVariable.onValueChanged += CheckPlayers;

        CheckPlayers();
    }

    private void OnDisable()
    {
        _server_StartClock.Unsubscribe(OnClockStarted);
        _server_EndClock.Unsubscribe(OnClockEnded);

        _playersVariable.onValueChanged -= CheckPlayers;
    }

    public void SetPlayersNeeded(int players)
    {
        _playersText.enabled = true;
        _playersText.text = players > 0 ? $"{players} more players" : "Starting match";
        _timeText.enabled = false;
    }

    private void OnClockStarted(int seconds)
    {
        _playersText.enabled = false;
        _timeText.enabled = true;
        Timing.RunCoroutine(StartCountdown(seconds), Segment.SlowUpdate, COROUTINE_TAG);
    }

    private void OnClockEnded(int winnerBall)
    {
        Debug.Log("WinnerBall: " + winnerBall);
    }

    private void CheckPlayers()
    {
        Debug.Log($"Checking Players {_playersVariable.Value} { _requiredPlayersVariable.Value}");
        if (_playersVariable.Value <= _requiredPlayersVariable.Value)
        {
            Timing.KillCoroutines(COROUTINE_TAG);
            SetPlayersNeeded(_requiredPlayersVariable.Value - _playersVariable.Value);
        }
        //else
        //{
        //    OnClockStarted(TIMER_SECONDS);
        //}
    }

    IEnumerator<float> StartCountdown(int TimerSeconds)
    {
        _startTime = Time.time;
        while(Time.time - _startTime <= TimerSeconds)
        {
            SetTimeText(new TimeSpan(0, 0, TimerSeconds - (int)(Time.time - _startTime)));
            yield return 0;
        }
        //_timeText.text = $"{0}";
        _timeText.text = $"Waiting for Results";
    }

    private void SetTimeText(TimeSpan span)
    {
        _timeText.enabled = true;
        if (span.TotalMinutes > 1)
            _timeText.text = $"{span.Minutes}: {span.Seconds}";
        else
            _timeText.text = $"{span.Seconds}";
    }
}
