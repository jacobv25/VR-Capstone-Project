using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public CheckpointManager checkpointManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            checkpointManager.CheckpointReached(this);
        }
    }
}
