using UnityEngine;
using Pixelplacement;
using BNG;
public class FlyingCarpetScript : MonoBehaviour
{
    public Spline route;
    public Transform flyingCarpet;
    public GameObject playerController;
    public float flightTime = 30; 

    private void Awake()
    {
        Tween.Spline(route, flyingCarpet, 0, 1, true, flightTime, 0, Tween.EaseInOut, Tween.LoopType.None);

        playerController.GetComponent<CharacterController>().enabled = false; //  disable character contoller

        playerController.GetComponent<PlayerTeleport>().enabled = false; //  disable player teleport

        playerController.GetComponent<LocomotionManager>().enabled = false; //  disable player locomotion manager

        playerController.GetComponent<SmoothLocomotion>().enabled = false; //  disable player smooth locomotion

        playerController.GetComponent<PlayerRotation>().enabled = false; //  disable player rotation

        playerController.GetComponent<PlayerGravity>().enabled = false;

    }
}
