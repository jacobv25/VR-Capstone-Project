using System.Collections;
using UnityEngine;

public class FollowPlayerMovement : IDroneMovement
{
    private DroneController droneController;
    private Vector3 previousPlayerPosition;
    private bool playerIsMoving = true;
    private Coroutine lookAtPlayerCoroutine;
    private Vector3 rotationDirection;
    private bool updateRotation = true;

    public FollowPlayerMovement(DroneController droneController)
    {
        this.droneController = droneController;
        previousPlayerPosition = droneController.target.position;
        rotationDirection = droneController.target.position + droneController.offset;
    }

    public void UpdateMovement(DroneController droneController)
    {
        Transform target = droneController.target;
        Vector3 targetPosition = target.position + droneController.offset;
        Vector3 smoothedPosition = Vector3.Lerp(droneController.transform.position, targetPosition, droneController.followSpeed * Time.deltaTime);
        droneController.transform.position = smoothedPosition;

        if (Vector3.Distance(previousPlayerPosition, target.position) > 0.001f)
        {
            playerIsMoving = true;
            previousPlayerPosition = target.position;
            rotationDirection = targetPosition;
            updateRotation = true;

            if (lookAtPlayerCoroutine != null)
            {
                droneController.StopCoroutine(lookAtPlayerCoroutine);
            }
            lookAtPlayerCoroutine = droneController.StartCoroutine(WaitAndLookAtPlayer());
        }
        else if (playerIsMoving)
        {
            playerIsMoving = false;

            if (lookAtPlayerCoroutine != null)
            {
                droneController.StopCoroutine(lookAtPlayerCoroutine);
            }
            lookAtPlayerCoroutine = droneController.StartCoroutine(WaitAndLookAtPlayer());
        }

        if (updateRotation)
        {
            Vector3 direction = rotationDirection - droneController.transform.position;
            if (direction.magnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                droneController.transform.rotation = Quaternion.Slerp(droneController.transform.rotation, targetRotation, droneController.rotationSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator WaitAndLookAtPlayer()
    {
        yield return new WaitForSeconds(droneController.waitTimeAfterPlayerStops);
        if (!playerIsMoving)
        {
            updateRotation = false;
            rotationDirection = droneController.target.position;
            Vector3 direction = rotationDirection - droneController.transform.position;
            if (direction.magnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                droneController.transform.rotation = Quaternion.Slerp(droneController.transform.rotation, targetRotation, droneController.rotationSpeed * Time.deltaTime);
            }
            updateRotation = true;
        }
    }
}
