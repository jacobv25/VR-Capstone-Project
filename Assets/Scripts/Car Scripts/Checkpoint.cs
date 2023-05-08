using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private RaceManager raceManager;

    private void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();
        if (raceManager == null)
        {
            Debug.LogError("No RaceManager found in the scene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            raceManager.CheckpointReached(this);
        }
    }
}