using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private BuildingCollision manager;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        manager = FindObjectOfType<BuildingCollision>();

        if (manager == null)
        {
            Debug.LogError("No BuildingCollision script found in the scene. Please add a manager object with the BuildingCollision script.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (manager != null)
        {
            manager.HandlePlayerCollision(collision, playerRigidbody);
        }
    }
}
