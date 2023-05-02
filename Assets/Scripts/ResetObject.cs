using UnityEngine;

public class ResetObject : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    void Start()
    {
        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Get the Rigidbody component if it exists
        rb = GetComponent<Rigidbody>();
    }

    public void ResetObjectTransform()
    {
        // Reset the position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset forces if Rigidbody component is present
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
