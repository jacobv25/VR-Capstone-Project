using UnityEngine;
using TMPro;
using BNG;
public class GlidingController : MonoBehaviour
{
    public TextMeshProUGUI debuggerText;
    public GameObject player;
    public float glideUpForce = 750.0f;
    public float glideForwardForce = 500.0f;
    public float distanceFromGroundAllowance = 0.5f;
    [HideInInspector] public Vector3 ForwardDirection;
    public BNGPlayerController playerController; // Reference to the BNGPlayerController script
    private Rigidbody playerRigidbody;
    private bool isGliding;
    private Vector3 upwardForce;
    private Vector3 forwardForce;

    private void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody component not found on the player.");
        }
        if (playerController == null)
        {
            Debug.LogError("BNGPlayerController not referenced. Please check the inspector.");

        }
    }

    private void Update()
    {
        if (isGliding)
        {
            ApplyGlideForce();
        }
    }

    public void StartGliding()
    {
        isGliding = true;
    }

    public void StopGliding()
    {
        isGliding = false;
    }

    private void ApplyGlideForce()
    {
        debuggerText.text = "distance from ground = " + playerController.DistanceFromGround;
        if (playerController.DistanceFromGround > distanceFromGroundAllowance)
        {
            float verticalVelocity = playerRigidbody.velocity.y;

            if (verticalVelocity < 0)
            {
                upwardForce = Vector3.up * glideUpForce;
                forwardForce = ForwardDirection * glideForwardForce;
                playerRigidbody.AddForce(upwardForce);
                playerRigidbody.AddForce(forwardForce);

                // Convert the force magnitudes to integers
                int upwardForceInteger = Mathf.RoundToInt(upwardForce.magnitude);
                int forwardForceInteger = Mathf.RoundToInt(forwardForce.magnitude);

            }
        }
    }

}
