using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DroneCommander : MonoBehaviour
{
    [Header("Drone Controller")]
    public Transform Drone;

    [Header("Ray Emitter")]
    public Transform RayEmitter;
    public Vector3 RayOffset; // Added offset property

    [Header("Ray Visualization Settings")]
    public int SegmentCount = 50;
    public float SegmentScale = 1;
    public LayerMask CollisionLayers;
    public LayerMask ValidLayers;

    [Header("Drone Command Indicator")]
    public GameObject DroneCommandIndicatorPrefab;

    private Vector3[] segments;
    private LineRenderer teleportLine;
    private GameObject droneCommandIndicator;
    private RaycastHit hit;

    private void Start()
    {
        segments = new Vector3[SegmentCount];
        teleportLine = GetComponent<LineRenderer>();

        if (teleportLine == null)
        {
            gameObject.AddComponent<LineRenderer>();
            teleportLine = GetComponent<LineRenderer>();
        }

        teleportLine.positionCount = SegmentCount;
        droneCommandIndicator = Instantiate(DroneCommandIndicatorPrefab);
        droneCommandIndicator.SetActive(false);
    }

    private void Update()
    {
        ShowRayVisualizer(); // Call this function to enable the ray visualizer
    }

    public void ShowRayVisualizer()
    {
        CastRayVisualizer();
    }

    private void CastRayVisualizer()
    {
        Vector3 worldSpaceOffset = RayEmitter.TransformDirection(RayOffset); // Convert local offset to world space
        segments[0] = RayEmitter.position + worldSpaceOffset; // Apply the world-space offset to the starting position
        Vector3 segVelocity = RayEmitter.forward * SegmentScale;

        for (int i = 1; i < SegmentCount; i++)
        {
            segVelocity = segVelocity + Physics.gravity * SegmentScale / segVelocity.magnitude;
            segments[i] = segments[i - 1] + segVelocity;

            if (Physics.Raycast(segments[i - 1], segVelocity, out hit, SegmentScale, CollisionLayers))
            {
                droneCommandIndicator.SetActive(true);
                droneCommandIndicator.transform.position = hit.point;
                droneCommandIndicator.transform.rotation = Quaternion.FromToRotation(droneCommandIndicator.transform.up, hit.normal) * droneCommandIndicator.transform.rotation;

                break;
            }
        }

        teleportLine.enabled = true;
        teleportLine.positionCount = SegmentCount;
        for (int i = 0; i < SegmentCount; i++)
        {
            teleportLine.SetPosition(i, segments[i]);
        }
    }

    // Call this function when you want to send the drone to the selected location
    public void SendDroneToTarget()
    {
        if (droneCommandIndicator.activeSelf)
        {
            Drone.position = droneCommandIndicator.transform.position;
        }
    }
}
