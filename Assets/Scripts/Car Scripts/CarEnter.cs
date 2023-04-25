using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CarEnter : MonoBehaviour
{
    public GameObject playerController; // the player controller
    public GameObject vehicle; //vehicle
    public GameObject carDestination; // the place the player goes in the car
    public GameObject rightHandModel;
    public GameObject leftHandModel;
    public GameObject enterText;

    Quaternion seatRotation; // rotation of player once in the car
    Vector3 seatPosition; //  position of player in the car

    public float carPlayerHeight = -0.35f;

    public bool playerIsInTheCube;

    public void Update()
    {
        if (playerIsInTheCube == true && InputBridge.Instance.RightTriggerDown)
        {
            seatRotation = carDestination.transform.rotation;
            seatPosition = carDestination.transform.position;


            playerController.transform.position = seatPosition; //  set position of player
            playerController.transform.rotation = seatRotation; //  set rotation of the player

            playerController.GetComponent<BNGPlayerController>().CharacterControllerYOffset = carPlayerHeight; // set height of player once in the car

            playerController.GetComponent<CharacterController>().enabled = false; //  disable character contoller

            playerController.GetComponent<PlayerTeleport>().enabled = false; //  disable player teleport

            playerController.GetComponent<LocomotionManager>().enabled = false; //  disable player locomotion manager

            playerController.GetComponent<SmoothLocomotion>().enabled = false; //  disable player smooth locomotion

            playerController.GetComponent<PlayerRotation>().enabled = false; //  disable player rotation

            playerController.GetComponent<PlayerGravity>().enabled = false; // disable player gravity



            //disable hand collision or suffer beating the car around while you are in it
            rightHandModel.GetComponent<HandCollision>().EnableCollisionOnPoint = false;
            rightHandModel.GetComponent<HandCollision>().EnableCollisionOnFist = false;
            leftHandModel.GetComponent<HandCollision>().EnableCollisionOnPoint = false;
            leftHandModel.GetComponent<HandCollision>().EnableCollisionOnFist = false;

            playerController.transform.parent = vehicle.transform; //set player contoller parent from XRrig to the vehicle

            carDestination.GetComponent<CarExit>().intheCar = true; // set bool on CarExit script to true

            vehicle.GetComponent<MyCarController>().intheCar = true;

            playerIsInTheCube = false;

            enterText.SetActive(false); // disable enter text

            gameObject.SetActive(false); // disable enter cube


        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsInTheCube = true;
            enterText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsInTheCube = false;
            enterText.SetActive(false);
        }
    }
}
