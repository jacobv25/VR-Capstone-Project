using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup1;
    [SerializeField] private GameObject buttonGroup2;

    public void StartGame()
    {
        buttonGroup1.SetActive(true);
        buttonGroup2.SetActive(false);
    }

    public void SelectTutorial()
    {
        buttonGroup1.SetActive(false);
        buttonGroup2.SetActive(true);
    }

    public void ShowMainMenu()
    {
        buttonGroup1.SetActive(true);
        buttonGroup2.SetActive(false);
    }
}
