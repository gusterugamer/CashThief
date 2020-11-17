using DG.Tweening;
using UnityEngine;

public class Anim_BallScaler : MonoBehaviour
{
    [SerializeField] private Vector2 _invokePeriod;
    [SerializeField] private Vector2 _durationRange;

    private void OnEnable()
    {
        Invoke("RunAnimRoutine", Random.Range(_invokePeriod.x, _invokePeriod.y));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void RunAnimRoutine()
    {
        var duration = Random.Range(_durationRange.x, _durationRange.y);
        var scale = Random.Range(.95f, 1.05f);
        transform.DOScale(scale, duration / 2f);
        transform.DOScale(1f, duration / 2f).SetDelay(duration / 2f);
        Invoke("RunAnimRoutine", Random.Range(_invokePeriod.x, _invokePeriod.y) + duration);
    }

}
