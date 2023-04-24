using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    // Create a serialized field to allow assigning an event in the Unity Inspector
    [SerializeField] private UnityEvent delayedEvent;

    // Set the delay time (in seconds)
    [SerializeField] private float delayTime = 30.0f;

    private void Start()
    {
        // Invoke the event after the specified delay time
        StartCoroutine(InvokeDelayedEvent());
    }

    private IEnumerator InvokeDelayedEvent()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Invoke the event
        delayedEvent.Invoke();
    }
}
