using UnityEngine;
using Pixelplacement;

public class CoinPlacer : MonoBehaviour
{
    public Spline raceRoute;
    public Transform coinPlacerObject;
    [Tooltip("How long it takes for the coin placer object to move through the route.\n" +
                "Longer = more coins. Shorter = less coins")]
    public int duration;

    void Awake()
    {
        Tween.Spline(raceRoute, coinPlacerObject, 0, 1, true, duration, 0, Tween.EaseLinear, Tween.LoopType.None);
    }
}