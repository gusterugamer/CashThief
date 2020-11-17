using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using DG.Tweening;
using JogaJoga;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UIBehaviour_Generic;

public class ChosenBall : MonoBehaviour
{
    [SerializeField] PopupEvent _popupEvent;
    [SerializeField] SimpleEvent _popupCloseEvent;
    [SerializeField] SimpleEvent _playerDiscardEvent;
    [SerializeField] IntVariable _pickedBallVariable;
    [SerializeField] Transform _ballTransform;
    [SerializeField] Transform _effectsTransform;

    [SerializeField] TextMeshProUGUI _ballNrText;
    [SerializeField] Image _ballImage;

    BallsContainer _container;
    [ShowInInspector, ReadOnly] private BallsContainer _Container => _container ?? (_container = GetComponentInParent<BallsContainer>());
    [ShowInInspector, ReadOnly] private LotteryBall _lastPickedObject;

    Tweener _selectionTween1;
    Tweener _selectionTween2;

    Tweener _deselectionTween1;
    Tweener _deselectionTween2;

    private void OnEnable()
    {
        _pickedBallVariable.onValueChanged += OnBallChosen;
    }

    private void OnDisable()
    {
        _pickedBallVariable.onValueChanged -= OnBallChosen;
    }

    private void OnBallChosen()
    {
        if (_pickedBallVariable.Value == 0)
            DeselectBall();
        else
            SelectBall();
    }

    public void CancelSelection()
    {
        _popupEvent.Invoke(
            $"Discard ball", 
            $"Are you sure you want to deselect ball nr {_pickedBallVariable.Value}", 
            new List<GenericButton>
            {
                new GenericButton("Yes", () => {
                    _pickedBallVariable.Value = 0;
                    _playerDiscardEvent.Invoke();
                    _popupCloseEvent.Invoke();
                }),
                new GenericButton("No", () => {
                    _popupCloseEvent.Invoke();
                }),
            }
        );
    }

    private void SelectBall()
    {
        // Kill prev tweens
        if (_deselectionTween1 != null && _deselectionTween1.active) _deselectionTween1.Kill();
        if (_deselectionTween2 != null && _deselectionTween2.active) _deselectionTween2.Kill();

        // Imitate normal ball
        _lastPickedObject = _Container.GetBall(_pickedBallVariable.Value);
        _lastPickedObject.DisplayBall(false);

        _ballTransform.localScale = Vector3.one * 0.75f;
        _ballTransform.position = _lastPickedObject.transform.position;
        _ballTransform.localPosition = new Vector3(_ballTransform.localPosition.x, _ballTransform.localPosition.y, 0f);
        _ballImage.sprite = _lastPickedObject.BallImage.sprite;

        _ballTransform.gameObject.SetActive(true);
        // Set Number;
        _ballNrText.text = _pickedBallVariable.Value.ToString();

        // Increase ball scale;
        _selectionTween1 = _ballTransform.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.InOutQuad);
        _selectionTween2 = _ballTransform.DOScale(1.5f, .25f).SetEase(Ease.InOutQuad);

        // Increase glow scale;
        _effectsTransform.gameObject.SetActive(true);
        _effectsTransform.localScale = Vector3.zero;
        _effectsTransform.DOScale(1.5f, .15f).SetEase(Ease.InOutQuad);
    }

    private void DeselectBall()
    {
        // Kill prev tweens
        if (_selectionTween1 != null && _selectionTween1.active) _selectionTween1.Kill();
        if (_selectionTween2 != null && _selectionTween2.active) _selectionTween2.Kill();

        // Imitate normal ball
        if (!_lastPickedObject)
            return;
        _deselectionTween1 = _ballTransform.DOMove(_lastPickedObject.transform.position, .25f);
        _deselectionTween2 = _ballTransform.DOScale(.75f, .25f).SetEase(Ease.InOutQuad).OnComplete(() => {
            _ballTransform.gameObject.SetActive(false);
            _effectsTransform.gameObject.SetActive(false);
            _lastPickedObject.DisplayBall(true);
        });

        // Decrease ball scale;
        _effectsTransform.DOScale(0f, .25f).SetEase(Ease.InOutQuad);
    }
}
