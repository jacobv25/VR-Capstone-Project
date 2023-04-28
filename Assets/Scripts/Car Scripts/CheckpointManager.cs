using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    public RaceManager raceManager;
    private int currentCheckpointIndex;

    private void Start()
    {
        currentCheckpointIndex = 0;
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        int checkpointIndex = System.Array.IndexOf(checkpoints, checkpoint);

        if (checkpointIndex == currentCheckpointIndex)
        {
            currentCheckpointIndex++;

            if (currentCheckpointIndex == checkpoints.Length)
            {
                currentCheckpointIndex = 0; // Reset the checkpoint index for the next lap
                raceManager.CompleteLap();
            }
        }
    }
}
