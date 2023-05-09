using UnityEngine;
using TMPro;
using System.Collections.Generic;
using BNG;
using System.Collections;

public class RaceManager : MonoBehaviour
{
    public int totalLaps;
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI lapCountText; 
    public TextMeshProUGUI finalTimeText; 
    [Header("Races")]
    public GameObject[] races;

    public CarExit carExit;

    private int currentLap;
    private float raceTime;
    private bool raceFinished;
    private List<Checkpoint> checkpoints;
    private int currentCheckpointIndex;
    private GameObject activeRace;
    private float[] bestTimes;
    private int selectedRaceIndex;
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();

        selectedRaceIndex = PlayerPrefs.GetInt("SelectedRaceIndex", 0); // Default to the first race if no index is found

        bestTimes = new float[RaceSelectionScreen.NUM_BEST_TIMES];
        for (int i = 0; i < RaceSelectionScreen.NUM_BEST_TIMES; i++)
        {
            // Load the best times for the selected race from PlayerPrefs (default to a large value if no best time is found)
            bestTimes[i] = PlayerPrefs.GetFloat("BestTime_" + selectedRaceIndex + "_" + i, Mathf.Infinity);
        }

        currentLap = 1;
        raceTime = 0f;
        currentCheckpointIndex = 0;
        raceFinished = false;
        checkpoints = new List<Checkpoint>();

        ActivateSelectedRace();
        foreach (Transform child in activeRace.transform)
        {
            Checkpoint checkpoint = child.GetComponent<Checkpoint>();
            if (checkpoint != null)
            {
                checkpoints.Add(checkpoint);
            }
        }
        MakeCheckPointVisible(checkpoints[currentCheckpointIndex]);
        UpdateLapCountText();
    }


    private void ActivateSelectedRace()
    {
        Debug.Log("race index = " + selectedRaceIndex);
        if (selectedRaceIndex < races.Length)
        {
            activeRace = races[selectedRaceIndex];
            activeRace.SetActive(true);
        }
        else
        {
            Debug.LogError("SelectedRaceIndex is out of bounds.");
        }
    }

    private void Update()
    {
        if (!raceFinished)
        {
            raceTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = (int)(raceTime / 60);
        int seconds = (int)(raceTime % 60);
        int milliseconds = (int)((raceTime * 100) % 100);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void UpdateLapCountText()
    {
        lapCountText.text = string.Format("Lap {0}/{1}", currentLap, totalLaps);
    }

    private void CompleteLap()
    {
        if (currentLap < totalLaps)
        {
            currentLap++;
            UpdateLapCountText();
        }
        else
        {
            raceFinished = true;
            float finalRaceTime = raceTime;
            finalTimeText.text = "Final Time: " + timerText.text;
            UpdateBestTimes(finalRaceTime);
            carExit.ExitCar();
            StartCoroutine(GoBackToGarage());
            
        }
    }

    private IEnumerator GoBackToGarage()
    {
        yield return new WaitForSeconds(1.0f);
        //go back to garage scene
        sceneLoader.LoadScene("Garage Scene 1");
    }

    private void UpdateBestTimes(float raceTime)
    {
        int selectedRaceIndex = PlayerPrefs.GetInt("SelectedRaceIndex", 0);
        for (int i = 0; i < RaceSelectionScreen.NUM_BEST_TIMES; i++)
        {
            if (raceTime < bestTimes[i])
            {
                // Shift the best times down the list and insert the new time
                for (int j = RaceSelectionScreen.NUM_BEST_TIMES - 1; j > i; j--)
                {
                    bestTimes[j] = bestTimes[j - 1];
                }
                bestTimes[i] = raceTime;

                // Save the updated best times for the selected race in PlayerPrefs
                for (int j = 0; j < RaceSelectionScreen.NUM_BEST_TIMES; j++)
                {
                    PlayerPrefs.SetFloat("BestTime_" + selectedRaceIndex + "_" + j, bestTimes[j]);
                }
                PlayerPrefs.Save();
                break;
            }
        }
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        //print if checkpoint is null
        if (checkpoint == null)
        {
            Debug.LogError("Checkpoint is null");
        }
        //print is checkpoints is null
        if (checkpoints == null)
        {
            Debug.LogError("Checkpoints List is null");
        }
        int checkpointIndex = checkpoints.IndexOf(checkpoint);
        // Racer drove through the correct checkpoint
        if (checkpointIndex == currentCheckpointIndex)
        {
            currentCheckpointIndex++;
    
            if (currentCheckpointIndex == checkpoints.Count)
            {
                currentCheckpointIndex = 0;
                CompleteLap();
            }
            MakeCheckPointVisible(checkpoints[currentCheckpointIndex]);
            MakeCheckpointInactive(checkpoints[checkpointIndex]);
        }
    }

    public void MakeCheckpointInactive(Checkpoint checkpoint)
    {
        int checkpointIndex = checkpoints.IndexOf(checkpoint);
        checkpoints[checkpointIndex].gameObject.SetActive(false);
    }

    public void MakeCheckPointVisible(Checkpoint checkpoint)
    {
        int checkpointIndex = checkpoints.IndexOf(checkpoint);
        checkpoints[checkpointIndex].gameObject.SetActive(true);
        checkpoints[checkpointIndex].GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.5f);
    }
}
