using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RobotFlyingSound : MonoBehaviour
{
    public Rigidbody robotRigidbody;
    public float maxSpeed = 10f;
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;
    [Range(0f, 1f)] public float volume = 1f;
    public float pitchMultiplier = 1f; // Controls how drastic the pitch change is relative to robot movement

    private AudioSource audioSource;
    private Vector3 previousPosition;

    private void Start()
    {
        previousPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }

    private void Update()
    {
        float currentSpeed = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;

        float pitch = Mathf.Lerp(minPitch, maxPitch, (currentSpeed / maxSpeed) * pitchMultiplier);

        audioSource.pitch = pitch;
    }
}
