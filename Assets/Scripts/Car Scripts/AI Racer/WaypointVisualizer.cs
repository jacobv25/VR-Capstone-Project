using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaypointVisualizer : MonoBehaviour
{
    public bool debug;

    void OnDrawGizmos()
    {
        if (debug)
        {
            int waypointCount = transform.childCount;
            if (waypointCount == 0)
            {
                return;
            }

            Gizmos.color = Color.black;

            for (int i = 0; i < waypointCount; i++)
            {
                Transform waypoint = transform.GetChild(i);

                // Draw waypoint position
                Gizmos.DrawSphere(waypoint.position, 0.5f);

                // Draw waypoint order label
                GUIStyle labelStyle = new GUIStyle();
                labelStyle.normal.textColor = Color.black;
                UnityEditor.Handles.Label(waypoint.position, (i).ToString(), labelStyle);

                // Draw line to next waypoint
                Transform nextWaypoint = transform.GetChild((i + 1) % waypointCount);
                Gizmos.DrawLine(waypoint.position, nextWaypoint.position);
            }
        }
        
    }
}
