using DG.Tweening;
using UnityEngine;

public class Anim_Rotator : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _value;

    void OnEnable()
    {
        var target = Vector3.forward * _value;
        transform.DOLocalRotate(target, _duration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }
}
