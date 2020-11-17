using Blastproof.Systems.Core;
using Blastproof.Tools.Elements;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MixerBall : ImageElement
{
    public int Number;

    private BallsContainer _ballsContainer;
    private BallsContainer _BallsContainer => _ballsContainer ?? (_ballsContainer = GetComponentInParent<BallsContainer>());

    Vector3 _startLocalPosition;

    const float OFFSET = 5;

    private void Start()
    {
        _startLocalPosition = transform.localPosition;
    }

    [Button]
    private void MoveToRandomPosition()
    {
        var rp = RandomizedPosition();
        transform.DOLocalMove(rp, 1f);
    }

    [Button]
    private Vector3 RandomizedPosition()
    {
        var width = ThisImage.rectTransform.GetWidth() / 2f;
        var radius = ThisImage.rectTransform.parent.GetComponent<RectTransform>().GetWidth() / 2f;
        var distance = radius - width - OFFSET;
        var randomPosition = new Vector2(Random.Range(-distance, distance), Random.Range(-distance, distance));
        //ThisImage.rectTransform.localPosition = randomPosition;

        while (Vector3.Distance(randomPosition, transform.parent.localPosition) > distance)
            RandomizedPosition();

        return randomPosition;
    }

    [Button]
    private void RandomSprite() { ThisImage.sprite = _BallsContainer.GetBall(Number).BallImage.sprite; }

    [Button]
    private void SetNumber() { Number = name.Split(' ')[1].Replace("(", "").Replace(")", "").To<int>(); }

    [Button]
    private void SetNumberText() { GetComponentInChildren<TMPro.TextMeshProUGUI>(true).text = Number.ToString(); }
}
