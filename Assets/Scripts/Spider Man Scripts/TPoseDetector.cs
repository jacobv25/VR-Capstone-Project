using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TPoseDetector : MonoBehaviour
{
    public TextMeshProUGUI debuggerText;

    public TPoseCalibration handCalibration;
    public GlidingController glidingController; // Add a reference to the GlidingController script

    public float handHeightTolerance = 0.15f;
    public float handDistanceTolerance = 0.15f;
    public Transform leftHand;
    public Transform rightHand;

    public UnityEvent onTPoseDetected;
    public UnityEvent onTPoseReleased;

    private bool tPoseActive;
    private Vector3 forwardDirection;

    private LineRenderer rayVisualizer;

    private void Start()
    {
        rayVisualizer = GetComponent<LineRenderer>();

        // Initialize the line renderer
        if (rayVisualizer == null)
        {
            Debug.LogError("LineRenderer not attached!");
        }
        rayVisualizer.startWidth = 0.05f;
        rayVisualizer.endWidth = 0.05f;
        rayVisualizer.positionCount = 2;
    }

    private void Update()
    {
        bool isInTPose = IsPlayerInTPose();

        if (isInTPose && !tPoseActive)
        {
            tPoseActive = true;
            onTPoseDetected.Invoke();
        }
        else if (!isInTPose && tPoseActive)
        {
            tPoseActive = false;
            onTPoseReleased.Invoke();
            rayVisualizer.enabled = false; // Hide the line renderer when not in T-pose
        }

        if (tPoseActive)
        {
            UpdateForwardDirectionAndDrawRay();
        }
    }

    private void UpdateForwardDirectionAndDrawRay()
    {
        // Calculate the vector between the player's hands
        Vector3 handDirection = (rightHand.position - leftHand.position).normalized;

        // Find the horizontal vector that is perpendicular to the line between the hands
        forwardDirection = Vector3.Cross(handDirection, Vector3.up).normalized;
        glidingController.ForwardDirection = forwardDirection;

        // Calculate position between hands
        Vector3 centerPosition = Vector3.Lerp(leftHand.position, rightHand.position, 0.5f);

        // Draw Helper Ray using LineRenderer
        rayVisualizer.enabled = true;
        rayVisualizer.SetPosition(0, centerPosition);
        rayVisualizer.SetPosition(1, centerPosition + forwardDirection * 2); // Adjust the scaling factor as needed (here, it's set to 2)
    }

    private bool IsPlayerInTPose()
    {
        float handDistance = Vector3.Distance(leftHand.position, rightHand.position);
        bool handsFarApart = handDistance >= (handCalibration.handDistanceApart - handDistanceTolerance) && handDistance <= (handCalibration.handDistanceApart + handDistanceTolerance);
        if(handsFarApart)
        {
            debuggerText.text = "Player is in TPose";
        }
        else
        {
            debuggerText.text = "";
        }
        return handsFarApart;
    }
}
