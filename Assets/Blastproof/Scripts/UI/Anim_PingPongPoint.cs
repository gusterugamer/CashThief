using UnityEngine;
using DG.Tweening;

public class Anim_PingPongPoint : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _time;
    [SerializeField] private Ease _ease;
    [SerializeField] private LoopType _loop;

    private void Start()
    {
        transform.DOLocalMove(_point.transform.localPosition, _time).SetEase(_ease).SetLoops(-1, _loop);
    }
}
