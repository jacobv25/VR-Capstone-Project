using UnityEngine;
using BNG;

public class DroneController : MonoBehaviour
{
    public RaycastWeapon weapon;
    public Transform weaponTransform; // Add this variable to hold the weapon transform

    public Light illuminationLight;
    public float illuminationIntensity = 1.0f;
    public float illuminationDistanceThreshold = 10.0f;

    public float followSpeed = 3.0f;
    public float rotationSpeed = 2.0f;
    public Vector3 offset;
    public float waitTimeAfterPlayerStops = 1f;

    public Transform target; // The target (player) the drone should follow or point at
    public Vector3 targetLocation; // The location the drone should move to when given a command
    
    public DroneState currentState; // The current state of the drone

    private IDroneMovement currentMovement; // The current movement behavior of the drone

    // Start is called before the first frame update
    private void Start()
    {
        SetState(new FollowPlayerState(this)); // Set the drone's initial state to follow the player
    }

    // FixedUpdate is called at fixed intervals
    private void FixedUpdate()
    {
        if (currentMovement != null) // Check if there is a current movement behavior
        {
            currentMovement.UpdateMovement(this); // Update the drone's movement behavior
        }
    }

    // Sets the current state of the drone to the new state provided
    public void SetState(DroneState newState)
    {
        if (currentState != null) // Check if there is a current state
        {
            currentState.ExitState(); // Exit the current state
        }

        currentState = newState; // Set the new state as the current state
        currentState.EnterState(); // Enter the new state
    }

    // Sets the current movement behavior of the drone
    public void SetMovement(IDroneMovement movement)
    {
        currentMovement = movement; // Assign the new movement behavior to the drone
    }

    // Call this method to transition to the FightState
    public void EngageEnemy(GameObject enemy)
    {
        SetState(new FightState(this, enemy, weaponTransform));
    }

    public void ShootWeapon()
    {
        weapon.Shoot();
    }
}
