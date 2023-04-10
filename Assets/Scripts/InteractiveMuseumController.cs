using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveMuseumController : MonoBehaviour
{
    [SerializeField] private GameObject[] paintingSets; // Assign the sets of paintings in the Unity Editor
    private int activeSetIndex = 0;

    private void Start()
    {
        // Deactivate all painting sets at start
        foreach (GameObject set in paintingSets)
        {
            set.SetActive(false);
        }

        // Activate the first painting set
        paintingSets[activeSetIndex].SetActive(true);
    }

    // Call this function when button 1 is pressed
    public void OnButton1Press()
    {
        ChangeActivePaintingSet(0);
    }

    // Call this function when button 2 is pressed
    public void OnButton2Press()
    {
        ChangeActivePaintingSet(1);
    }

    // Call this function when button 3 is pressed
    public void OnButton3Press()
    {
        ChangeActivePaintingSet(2);
    }

    private void ChangeActivePaintingSet(int newIndex)
    {
        if (newIndex < 0 || newIndex >= paintingSets.Length || newIndex == activeSetIndex)
        {
            return;
        }

        paintingSets[activeSetIndex].SetActive(false);
        activeSetIndex = newIndex;
        paintingSets[activeSetIndex].SetActive(true);
    }
}
