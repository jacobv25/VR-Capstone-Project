using UnityEngine;
using TMPro;
public class RaceManager : MonoBehaviour
{
    public int totalLaps;
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI lapCountText; 
    public TextMeshProUGUI finalTimeText; 

    private int currentLap;
    private float raceTime;
    private bool raceFinished;

    private void Start()
    {
        currentLap = 1;
        raceTime = 0f;
        raceFinished = false;
        UpdateLapCountText();
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

    public void CompleteLap()
    {
        if (currentLap < totalLaps)
        {
            currentLap++;
            UpdateLapCountText();
        }
        else
        {
            raceFinished = true;
            finalTimeText.text = "Final Time: " + timerText.text;
        }
    }
}
