using UnityEngine;
using Pixelplacement;

public class DrivingHelperSplineTweener : MonoBehaviour
{
    public Spline raceTrackSpline;
    public Transform helperObjecct;
    public int duration;

    void Awake()
    {
        Tween.Spline(raceTrackSpline, helperObjecct, 0, 1, true, duration, 0, Tween.EaseLinear, Tween.LoopType.None);
    }
}