using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Car;
using TMPro;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AIRacerController1 : MonoBehaviour
{
    public List<Transform> waypoints;
    public TextMeshProUGUI debugger;
    public bool debug;
    [Header("Waypoint Reached Radius")]
    public float waypointReachedRadius = 5f; // Add this variable to specify the when a car has reached a waypoint

    [Header("Speed = 1 / StoppingDistance")]
    public Rigidbody carRigidbody;
    public float minStoppingDistance = 5f;
    public float maxStoppingDistance = 20f;
    public float minSpeed = 0f;
    public float maxSpeed = 10f;

    [Header("Look Ahead Distance")]
    public float lookAheadDistance1 = 5f;
    public float lookAheadDistance2 = 10f;
    
    private float previousSpeed;
    private float smoothedAcceleration;
    private NavMeshAgent navMeshAgent;
    private CarController carController;
    private int currentWaypoint;
    private int nextWaypoint;
    private bool isCoroutineRunning;
    private float fixedLength = 5f; // Adjust this value to change the length of the direction gizmo
    private float nextTurnAngle;




    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        carController = GetComponent<CarController>();

        navMeshAgent.speed = maxSpeed;

        currentWaypoint = 0;
        StartCoroutine(UpdateWaypoint());
    }

    private float currentSpeed;

    void Update()
    {
        // Get the current speed of the car
        currentSpeed = carRigidbody.velocity.magnitude;
        // Calculate acceleration
        float acceleration = (currentSpeed - previousSpeed) / Time.deltaTime;
        // Calculate the stopping distance based on the acceleration
        float stoppingDistance = Mathf.Lerp(minStoppingDistance, maxStoppingDistance, Mathf.InverseLerp(minSpeed, maxSpeed, acceleration));
        // Update the NavMeshAgent's stopping distance
        navMeshAgent.stoppingDistance = stoppingDistance;

        Vector3 desiredVelocity = navMeshAgent.desiredVelocity;
        float inputSteer = Vector3.Dot(transform.right, desiredVelocity.normalized);

        // Calculate the path's curvature by using multiple points along the path
        Vector3 lookAheadPoint1 = GetPointOnPath(lookAheadDistance1);
        Vector3 lookAheadPoint2 = GetPointOnPath(lookAheadDistance2);
        Vector3 lookAheadDirection = (lookAheadPoint2 - lookAheadPoint1).normalized;
        float curvatureFactor = Vector3.Dot(transform.forward, lookAheadDirection);

        // Adjust acceleration based on the curvature factor
        float desiredAcceleration = Mathf.Clamp(curvatureFactor, -1f, 1f);
        float inputAcceleration = Vector3.Dot(transform.forward, desiredVelocity.normalized) * desiredAcceleration;

        carController.Move(inputSteer, inputAcceleration, inputAcceleration, 0f);

        float angle = CurrentTurnAngle(waypoints[currentWaypoint].position, waypoints[nextWaypoint].position, transform.position);


        // Check if the car is close enough to the waypoint
        if (navMeshAgent.remainingDistance < waypointReachedRadius)
        {
            // If the coroutine is not running, start it
            if (!isCoroutineRunning)
            {
                StartCoroutine(UpdateWaypoint());
            }
        }
        if (debug)
        {
            string accelerationStatus = inputAcceleration > 0 ? "Accelerating" : (inputAcceleration < 0 ? "Braking" : "Coasting");

            // Display on the debugger TextMeshProUGUI
            debugger.text = $"Speed: {currentSpeed:F2}\nSteer: {inputSteer}\nStatus: {accelerationStatus}\nWaypoint: {currentWaypoint}" +
                            $"\nAngle: {angle}";

        }

        // Store the current speed as the previous speed for the next frame
        previousSpeed = currentSpeed;
    }

    private IEnumerator UpdateWaypoint()
    {
        isCoroutineRunning = true;

        // Wait for a short duration before updating the waypoint
        yield return new WaitForSeconds(0.1f);

        // Check if there are any waypoints in the list
        if (waypoints.Count > 0)
        {            
            currentWaypoint = nextWaypoint;

            // Set the destination of the NavMeshAgent to the position of the new current waypoint
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);

            // Update the index tracking the next waypoint
            if (currentWaypoint == waypoints.Count - 1)
            {
                nextWaypoint = 0;
            }
            else
            {
                nextWaypoint = currentWaypoint + 1;
            }
        }

        isCoroutineRunning = false;
    }

    private float CurrentTurnAngle(Vector3 currentWaypointPos, Vector3 nextWaypointPos, Vector3 transformPos) 
    {
        Vector3 line1 = (currentWaypointPos - transformPos).normalized;
        Vector3 line2 = (currentWaypointPos - nextWaypointPos).normalized;

        return Vector3.SignedAngle(line1, line2, Vector3.up);
    }

    private Vector3 GetPointOnPath(float distanceFromCar)
    {
        // Check if the NavMeshAgent has a valid path with at least one corner
        if (navMeshAgent.path == null || navMeshAgent.path.corners.Length == 0)
        {
            return transform.position;
        }

        Vector3 pointOnPath = transform.position; // Initialize the point on the path to the car's current position
        float remainingDistance = distanceFromCar; // Set the remaining distance to the input distance
        int currentCorner = 0; // Start at the first corner of the path

        // Continue the loop as long as there's remaining distance and we haven't reached the last corner
        while (remainingDistance > 0 && currentCorner < navMeshAgent.path.corners.Length - 1)
        {
            // Calculate the vector from the current corner to the next corner
            Vector3 cornerToPoint = navMeshAgent.path.corners[currentCorner + 1] - navMeshAgent.path.corners[currentCorner];
            float cornerToPointDistance = cornerToPoint.magnitude; // Calculate the distance between the two corners

            // Check if the remaining distance is less than or equal to the distance between the two corners
            if (remainingDistance <= cornerToPointDistance)
            {
                // Set the point on the path to be the remaining distance along the vector between the two corners
                pointOnPath = navMeshAgent.path.corners[currentCorner] + cornerToPoint.normalized * remainingDistance;
                break; // Break out of the loop since we've found the point on the path
            }

            // Subtract the distance between the two corners from the remaining distance
            remainingDistance -= cornerToPointDistance;
            currentCorner++; // Move to the next corner
        }

        return pointOnPath; // Return the calculated point on the path
    }




#if UNITY_EDITOR
void OnDrawGizmos()
{
    if (navMeshAgent != null)
    {
        // // Draw a line from the car to the normalized NavMeshAgent's desired velocity (the direction it wants to move in)
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(transform.position, transform.position + navMeshAgent.desiredVelocity.normalized * fixedLength);

        // Draw a circle representing the NavMeshAgent's stopping distance
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, navMeshAgent.stoppingDistance);

        // Draw a circle representing the waypoint reached radius
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, waypointReachedRadius);

        // // Draw triangle lines between the car, current waypoint, and the next waypoint
        // if (waypoints.Count > 0)
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawLine(transform.position, waypoints[currentWaypoint].position);
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawLine(waypoints[currentWaypoint].position, waypoints[nextWaypoint].position);
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawLine(waypoints[nextWaypoint].position, transform.position);

        //     // Draw an arc between the red line and the green line
        //     Vector3 redLineDirection = (waypoints[currentWaypoint].position - transform.position).normalized;
        //     Vector3 greenLineDirection = (waypoints[nextWaypoint].position - transform.position).normalized;
        //     Vector3 blueLineDirection = (waypoints[currentWaypoint].position - waypoints[nextWaypoint].position).normalized;

        //     //green arc
        //     float angle = Vector3.SignedAngle(redLineDirection, greenLineDirection, Vector3.up);
        //     Handles.color = new Color(0, 1, 0, 0.1f); // Semi-transparent green
        //     Handles.DrawSolidArc(transform.position, Vector3.up, redLineDirection, angle, Vector3.Distance(transform.position, waypoints[currentWaypoint].position));
        //     //blue arc
        //     angle = Vector3.SignedAngle(redLineDirection, blueLineDirection, Vector3.up);
        //     Handles.color = new Color(0, 0, 1, 0.1f); // Semi-transparent blue
        //     Handles.DrawSolidArc(waypoints[currentWaypoint].position, Vector3.up, redLineDirection, -angle, Vector3.Distance(transform.position, waypoints[currentWaypoint].position));
        // }
    }
}
#endif

}