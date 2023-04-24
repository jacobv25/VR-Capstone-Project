using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BuildingCollision : MonoBehaviour
{
    // The minimum force required to cause damage
    public float forceThreshold = 10.0f;

    // The duration in seconds to display the notification
    public float notificationDuration = 3.0f;

    // The TextMeshProUGUI child that displays the notification
    public TextMeshProUGUI notificationText;

    public bool useHearts = false;

    // The Hearts parent object
    public GameObject heartsParent;

    // List to store heart GameObjects
    private List<GameObject> hearts;

    private void Start()
    {
        // Initialize the hearts list and populate it with the children of heartsParent
        if(useHearts) 
        { 
            hearts = new List<GameObject>();
            for (int i = 0; i < heartsParent.transform.childCount; i++)
            {
                hearts.Add(heartsParent.transform.GetChild(i).gameObject);
            }
        }
    }

    public void HandlePlayerCollision(Collision collision, Rigidbody playerRigidbody)
    {
        // Calculate the relative velocity of the collision
        float impactForce = collision.relativeVelocity.magnitude * playerRigidbody.mass;

        // Check if the impact force is greater than the threshold
        if (impactForce >= forceThreshold)
        {
            // Apply damage to the player or trigger other game events as needed
            
            // Display the notification for the specified duration
            notificationText.text = "Impact force: " + impactForce;
            StartCoroutine(DisplayNotification(notificationDuration));

            if (useHearts)
            {
                // Remove a heart (set it inactive) to simulate losing health
                for (int i = hearts.Count - 1; i >= 0; i--)
                {
                    if (hearts[i].activeInHierarchy)
                    {
                        hearts[i].SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    IEnumerator DisplayNotification(float duration)
    {
        yield return new WaitForSeconds(duration);
        notificationText.text = "";
    }
}
