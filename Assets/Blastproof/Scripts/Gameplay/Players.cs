using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using UnityEngine;

public class Players : MonoBehaviour
{
    [SerializeField] private IntEvent _server_OtherPlayerChoseBall;
    [SerializeField] private IntEvent _server_OtherPlayerDiscardedBall;

    [SerializeField] private IntEvent _client_PlayerChoseBall;
    [SerializeField] private SimpleEvent _client_PlayerDiscardedBall;
    [SerializeField] private StringEvent _server_PlayerChoseBallDeny;

    [SerializeField] private IntVariable _playersVariable;

    private void OnEnable()
    {
        _server_OtherPlayerChoseBall.Subscribe(OnPlayerChoseBall);
        _server_OtherPlayerDiscardedBall.Subscribe(OnPlayerDiscardedBall);

        _client_PlayerChoseBall.Subscribe(OnPlayerChoseBall);
        _client_PlayerDiscardedBall.Subscribe(OnPlayerDiscardedBall);
        _server_PlayerChoseBallDeny.Subscribe(OnPlayerDiscardedBall);
    }

    private void OnDisable()
    {
        _server_OtherPlayerChoseBall.Unsubscribe(OnPlayerChoseBall);
        _server_OtherPlayerDiscardedBall.Unsubscribe(OnPlayerDiscardedBall);

        _client_PlayerChoseBall.Unsubscribe(OnPlayerChoseBall);
        _client_PlayerDiscardedBall.Unsubscribe(OnPlayerDiscardedBall);
        _server_PlayerChoseBallDeny.Unsubscribe(OnPlayerDiscardedBall);
    }


    private void OnPlayerChoseBall(int ball)
    {
        //_playersVariable.Value++;
    }

    private void OnPlayerDiscardedBall() { OnPlayerDiscardedBall(-1); }
    private void OnPlayerDiscardedBall(string error) { OnPlayerDiscardedBall(-1); }
    private void OnPlayerDiscardedBall(int ball)
    {
        //_playersVariable.Value--;
    }
}
