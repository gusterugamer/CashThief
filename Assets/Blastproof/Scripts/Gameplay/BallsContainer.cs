using JogaJoga;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BallsContainer : SerializedMonoBehaviour
{
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<int, LotteryBall> lotteryBalls = new Dictionary<int, LotteryBall>();

    private void Start()
    {
        FillDictionary();
    }

    public LotteryBall GetBall(int nr)
    {
        return lotteryBalls[nr];
    }

    [Button]
    private void FillDictionary()
    {
        lotteryBalls.Clear();
        var balls = GetComponentsInChildren<LotteryBall>(true);
        foreach (var ball in balls)
        {
            var comp = ball.GetComponent<LotteryBall>();
            lotteryBalls.Add(comp.Number, comp);
        }
    }
}
