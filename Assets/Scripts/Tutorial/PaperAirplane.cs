using UnityEngine;
using TMPro;

public class PaperAirplane : MonoBehaviour
{
    [SerializeField] float levelFlightSpeed = 100.0f;
    [SerializeField] float rotationDamping = 100.0f;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] Vector3 localRotationOffset = new Vector3(0, 0, 0); // Add a rotation offset to adjust the model's orientation

    private Rigidbody rb;
    private bool flying = false;
    private Vector3 startPosition;

    private Quaternion startRotation;
    [SerializeField] private float maxPitchAngle = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            statusText.text = "velocity = " + rb.velocity.magnitude;
        }
    }

    public void ResetAirplane()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        flying = false;
        UpdateStatusText();
    }

    public void Release()
    {
        flying = true;
    }

    private void FixedUpdate()
    {
        if (flying)
        {
            // Calculate the target rotation to keep the airplane level
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity.normalized);
            float pitchAngle = Mathf.Clamp(targetRotation.eulerAngles.x, -maxPitchAngle, maxPitchAngle);
            targetRotation = Quaternion.Euler(pitchAngle, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            targetRotation.x = 0; // Keep the X rotation fixed to prevent roll

            // Add the local rotation offset to the target rotation
            targetRotation *= Quaternion.Euler(localRotationOffset);

            // Smoothly adjust the airplane's rotation to match the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationDamping * levelFlightSpeed);
        }
    }

    private void Update()
    {
        UpdateStatusText();

        // Draw a line from the airplane's center in the direction parallel to the wings
        if (flying)
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * 10f, Color.green);
            Debug.DrawLine(transform.position, transform.position - transform.right * 10f, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * 10f, Color.red);
            Debug.DrawLine(transform.position, transform.position - transform.right * 10f, Color.red);
        }
    }
}
