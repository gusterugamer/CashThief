using UnityEngine;
using DG.Tweening;
using Blastproof.Tools.Elements;

public class Anim_BallGlowRotator : ImageElement
{
    [SerializeField] private Vector2 _invokePeriod;

    [SerializeField] private Vector2 _rotRange;
    [SerializeField] private Vector2 _speedRange;

    private void OnEnable()
    {
        Invoke("RunAnimRoutine", Random.Range(_invokePeriod.x, _invokePeriod.y));
    }

    void RunAnimRoutine()
    {
        var duration = Random.Range(_speedRange.x, _speedRange.y);
        var targetZ = transform.eulerAngles.z + Random.Range(_rotRange.x, _rotRange.y);

        transform.DORotate(new Vector3(0, 0, targetZ), duration, RotateMode.LocalAxisAdd);
        ThisImage.CrossFadeAlpha(1, .1f, false);
        ThisImage.DOFade(1f, duration / 2f);
        ThisImage.DOFade(0f, duration / 2f).SetDelay(duration / 2f);

        Invoke("RunAnimRoutine", Random.Range(_invokePeriod.x, _invokePeriod.y) + duration);
    }
}
