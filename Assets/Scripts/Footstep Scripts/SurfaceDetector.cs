// SurfaceDetector.cs
using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    public SurfaceType surfaceType = SurfaceType.Default;

    private void OnTriggerEnter(Collider other)
    {
        FootstepController footstepController = other.GetComponent<FootstepController>();
        if (footstepController != null)
        {
            footstepController.SetCurrentSurfaceType(surfaceType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FootstepController footstepController = other.GetComponent<FootstepController>();
        if (footstepController != null)
        {
            footstepController.SetCurrentSurfaceType(SurfaceType.Default);
        }
    }
}
