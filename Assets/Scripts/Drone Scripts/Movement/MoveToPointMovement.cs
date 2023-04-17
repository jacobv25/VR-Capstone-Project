// MoveToPointMovement.cs
using UnityEngine;

public class MoveToPointMovement : IDroneMovement
{
    public float moveToSpeed = 2.0f;

    public void UpdateMovement(DroneController droneController)
    {
        Vector3 smoothedPosition = Vector3.Lerp(droneController.transform.position, droneController.targetLocation, moveToSpeed * Time.deltaTime);
        droneController.transform.position = smoothedPosition;
    }
}