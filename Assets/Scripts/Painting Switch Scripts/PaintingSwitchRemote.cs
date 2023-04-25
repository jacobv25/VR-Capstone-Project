using System.Collections.Generic;
using UnityEngine;
using BNG;

public class PaintingSwitchRemote : MonoBehaviour
{
    public List<GameObject> paintingSets;
    public List<GameObject> buttonSets;
    public Button switchButton;

    private int currentSet;

    private void Start()
    {
        currentSet = 0;
        UpdateSets();

        switchButton.onButtonDown.AddListener(SwitchPaintings);
    }

    private void SwitchPaintings()
    {
        currentSet = (currentSet + 1) % paintingSets.Count;
        UpdateSets();
    }

    private void UpdateSets()
    {
        for (int i = 0; i < paintingSets.Count; i++)
        {
            paintingSets[i].SetActive(i == currentSet);
        }

        for (int i = 0; i < buttonSets.Count; i++)
        {
            buttonSets[i].SetActive(i == currentSet);
        }
    }
}
