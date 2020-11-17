using DG.Tweening;
using UnityEngine;

public class Anim_ScalePulser : MonoBehaviour
{
    [SerializeField] private float _targetSize;
    [SerializeField] private float _duration;

    void OnEnable()
    {
        transform.DOScale(_targetSize, _duration).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }
}
