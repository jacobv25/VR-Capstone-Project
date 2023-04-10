using UnityEngine;

public class TPoseCalibration : MonoBehaviour
{
    public Transform head;
    public Transform leftCalibrator;
    public Transform rightCalibrator;

    [HideInInspector] public float handDistanceFromHead = 0f;
    [HideInInspector] public float handDistanceApart = 0f;

    public void Calibrate()
    {
        // Calculate the distance between the left hand and head
        float leftHandHeadDistance = Vector3.Distance(leftCalibrator.position, head.position);

        // Calculate the distance between the right hand and head
        float rightHandHeadDistance = Vector3.Distance(rightCalibrator.position, head.position);

        // Calculate the average distance between hands and head
        handDistanceFromHead = (leftHandHeadDistance + rightHandHeadDistance) / 2.0f;

        // Calculate the distance between the left and right hands
        handDistanceApart = Vector3.Distance(leftCalibrator.position, rightCalibrator.position);

        Debug.Log("TPose Calibrated!");
        Debug.Log("          hand distance =" + handDistanceApart);
        Debug.Log("hand distance from head =" + handDistanceFromHead);
    }
}
