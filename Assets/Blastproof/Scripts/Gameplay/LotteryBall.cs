using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.Tools.Elements;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace JogaJoga
{
    public class LotteryBall : ButtonElement
    {
        [BoxGroup("Data"), SerializeField] public int Number;

        [BoxGroup("Data"), SerializeField] private Sprite[] _sprites;
        [BoxGroup("Data"), SerializeField] private IntVariable _pickedBall;

        [BoxGroup("Events"), SerializeField] private IntEvent _server_OtherPlayersChoseBall;
        [BoxGroup("Events"), SerializeField] private IntEvent _server_OtherPlayersDiscardedBall;
        [BoxGroup("Events"), SerializeField] private IntEvent _client_playerChoseBall;
        [BoxGroup("Events"), SerializeField] private SimpleEvent _gameResetEvent;

        private Image _ballImage;
        [BoxGroup("Info"), ShowInInspector, ReadOnly] public Image BallImage => _ballImage ?? (_ballImage = transform.GetChild(1).GetComponent<Image>());

        private Image _ballShadow;
        [BoxGroup("Info"), ShowInInspector, ReadOnly] public Image BallShadow => _ballShadow ?? (_ballShadow = transform.GetChild(0).GetComponent<Image>());

        [BoxGroup("Info"), ShowInInspector, ReadOnly] private bool _chosenByOthers;

        protected override void OnEnable()
        {
            base.OnEnable();
            _server_OtherPlayersChoseBall.Subscribe(OnOtherPlayersChoseBall);
            _server_OtherPlayersDiscardedBall.Subscribe(OnOtherPlayersDiscardedBall);
            _gameResetEvent.Subscribe(Reset);
            _pickedBall.onValueChanged += OnPickedBallChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _server_OtherPlayersChoseBall.Unsubscribe(OnOtherPlayersChoseBall);
            _server_OtherPlayersDiscardedBall.Unsubscribe(OnOtherPlayersDiscardedBall);
            _gameResetEvent.Unsubscribe(Reset);
            _pickedBall.onValueChanged -= OnPickedBallChanged;
        }

        protected override void OnButtonClick()
        {
            // If ball's already been picked by the player and this is a mere confirmation from the server, do nothing
            if (_pickedBall.Value != 0)
                return;

            // If ball's already been picked by other players, do nothing.
            if (_chosenByOthers) return;

            _pickedBall.Value = Number;
            _client_playerChoseBall?.Invoke(Number);
        }

        private void OnPickedBallChanged()
        {
            if (_chosenByOthers) return;
            if (_pickedBall.Value == 0) MarkUnchosen();
        }

        [Button]
        public void Reset() { MarkUnchosen(); _chosenByOthers = false; }

        public void DisplayBall(bool display)
        {
            BallImage.gameObject.SetActive(display);
            BallShadow.gameObject.SetActive(display);
        }

        private void OnOtherPlayersChoseBall(int number)
        {
            // Do nothing if number not this ball picked
            if (Number != number) return;

            _chosenByOthers = true;

            MarkChosenByOpponents();
        }

        private void OnOtherPlayersDiscardedBall(int number)
        {
            // Do nothing if number not this ball picked
            if (Number != number) return;

            _chosenByOthers = false;

            MarkUnchosen();
        }

        private void MarkChosenByOpponents() { BallImage.color = Color.gray; }
        private void MarkUnchosen() { BallImage.color = Color.white; }

        [Button]
        private void RandomSprite() { BallImage.sprite = _sprites.Random(); }

        [Button]
        private void SetNumber() { Number = name.Split(' ')[1].Replace("(", "").Replace(")", "").To<int>(); }

        [Button]
        private void SetNumberText() { GetComponentInChildren<TMPro.TextMeshProUGUI>(true).text = Number.ToString(); }
    }
}