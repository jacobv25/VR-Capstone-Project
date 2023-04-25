using UnityEngine;
using BNG;

public class TrackpadMovementDisplayHelper : MonoBehaviour
{
    [SerializeField] private float maxDistance = 1.0f;
    private InputBridge input;
    private Vector3 initialPosition;

    private void Start()
    {
        input = InputBridge.Instance;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // Get trackpad input
        Vector2 trackpadInput = input.LeftThumbstickAxis;

        // Move the object based on the thumb position on the trackpad
        Vector3 newPosition = initialPosition + new Vector3(trackpadInput.x * maxDistance, 0, trackpadInput.y * maxDistance);
        transform.localPosition = newPosition;
    }
}
