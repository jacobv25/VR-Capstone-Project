using UnityEngine;
using BNG;
using TMPro;
using System.Text;

public class RaceSelectionScreen : MonoBehaviour
{
    public GameObject AreYouSureScreen;
    public GameObject SelectionButtons;
    private TextMeshProUGUI bestTimeText;
    private SceneLoader sceneLoader;
    private float bestTime;
    public const int NUM_BEST_TIMES = 10;
    private float[] bestTimes = new float[NUM_BEST_TIMES];
    private const int NUM_RACES = 2;

    private void Start() 
    {

        bestTimeText = GameObject.Find("Highscore Text (TMP)").GetComponent<TextMeshProUGUI>();
        if(bestTimeText == null)
        {
            Debug.LogError("Cannot find Highscore Text (TMP) in scene");
        }
        sceneLoader = GetComponent<SceneLoader>();
        

        UpdateBestTimesText();
    }

    private void LoadBestTimesForSelectedRace()
    {
        int selectedRace = PlayerPrefs.GetInt("SelectedRaceIndex", 0);

        for (int i = 0; i < NUM_BEST_TIMES; i++)
        {
            // Load the best times from PlayerPrefs (default to a large value if no best time is found)
            bestTimes[i] = PlayerPrefs.GetFloat("BestTime_" + selectedRace + "_" + i, Mathf.Infinity);
            Debug.Log("BestTime_" + selectedRace + "_" + i + "=" + bestTimes[i]);
        }
    }

    public void SelectRace(int raceIndex)
    {
        PlayerPrefs.SetInt("SelectedRaceIndex", raceIndex);
        PlayerPrefs.Save();
        LoadBestTimesForSelectedRace();
        UpdateBestTimesText();
        AreYouSureScreen.SetActive(true);
        SelectionButtons.SetActive(false);
    }

    public void GoToDriveScene()
    {
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
