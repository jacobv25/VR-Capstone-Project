// MoveToPointAndIlluminateMovement.cs
using UnityEngine;

public class MoveToPointAndIlluminateMovement : IDroneMovement
{
    private Light illuminationLight; 
    private float illuminationIntensity; // The intensity of the light when illuminating the area
    private float illuminationDistanceThreshold; // The distance threshold for the drone to start illuminating

    private float originalIntensity; // The original intensity of the light

    public MoveToPointAndIlluminateMovement(DroneController droneController)
    {
        // Initialize the Light component and its original intensity
        illuminationLight = droneController.illuminationLight;
        originalIntensity = illuminationLight.intensity;

        // Initialize the illuminationIntensity and illuminationDistanceThreshold
        illuminationIntensity = droneController.illuminationIntensity;
        illuminationDistanceThreshold = droneController.illuminationDistanceThreshold;
    }

    public void UpdateMovement(DroneController droneController)
    {
        // Calculate the target position for the drone
        Vector3 targetPosition = droneController.targetLocation + new Vector3(1, 1, 0) * droneController.illuminationDistanceThreshold;

        // Move towards the target position
        droneController.transform.position = Vector3.MoveTowards(droneController.transform.position, targetPosition, droneController.followSpeed * Time.deltaTime);

        // Calculate the direction vector pointing from the drone to the target location
        Vector3 direction = (droneController.targetLocation - droneController.transform.position).normalized;

        // Create a rotation that looks at the target location
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate the drone to face the target location
        droneController.transform.rotation = Quaternion.Slerp(droneController.transform.rotation, targetRotation, droneController.rotationSpeed * Time.deltaTime);
    }
}
