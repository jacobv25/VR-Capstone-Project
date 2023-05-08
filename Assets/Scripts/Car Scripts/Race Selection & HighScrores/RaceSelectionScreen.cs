using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;
using System.Text;

public class RaceSelectionScreen : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;

    private SceneLoader sceneLoader;
    private float bestTime;
    public const int NUM_BEST_TIMES = 10;
    private float[] bestTimes;


    private void Start() 
    {
        sceneLoader = GetComponent<SceneLoader>();
        
        bestTimes = new float[RaceSelectionScreen.NUM_BEST_TIMES];
        for (int i = 0; i < RaceSelectionScreen.NUM_BEST_TIMES; i++)
        {
            // Load the best times from PlayerPrefs (default to a large value if no best time is found)
            bestTimes[i] = PlayerPrefs.GetFloat("BestTime_" + i, Mathf.Infinity);
        }
    }

    public void SelectRace(int raceIndex)
    {
        PlayerPrefs.SetInt("SelectedRaceIndex", raceIndex);
        PlayerPrefs.Save();

        //Load Driving Scene
        sceneLoader.LoadScene("Driving City");
    }

    private void UpdateBestTimesText()
    {
        StringBuilder bestTimesBuilder = new StringBuilder("Best Times:\n");
        for (int i = 0; i < NUM_BEST_TIMES; i++)
        {
            if (bestTimes[i] == Mathf.Infinity)
            {
                bestTimesBuilder.Append("--:--:--\n");
            }
            else
            {
                int minutes = (int)(bestTimes[i] / 60);
                int seconds = (int)(bestTimes[i] % 60);
                int milliseconds = (int)((bestTimes[i] * 100) % 100);
                bestTimesBuilder.AppendFormat("{0:00}:{1:00}:{2:00}\n", minutes, seconds, milliseconds);
            }
        }
        bestTimeText.text = bestTimesBuilder.ToString();
    }

}
