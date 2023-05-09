
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class PaintingSwitchRemote : MonoBehaviour
{
    public List<GameObject> paintingSets;
    public List<GameObject> buttonSets;
    public List<Button> switchButtons;

    private void Start()
    {
        InitializeButtons();
        UpdateSetsAndButtons(0);
    }

    private void InitializeButtons()
    {
        for (int i = 0; i < switchButtons.Count; i++)
        {
            int index = i;
            switchButtons[i].onButtonDown.AddListener(() => SwitchPaintings(index));
        }
    }

    private void SwitchPaintings(int index)
    {
        UpdateSetsAndButtons(index);
    }

    private void UpdateSetsAndButtons(int activeSet)
    {
        for (int i = 0; i < paintingSets.Count; i++)
        {
            paintingSets[i].SetActive(i == activeSet);
        }

        for (int i = 0; i < buttonSets.Count; i++)
        {
            buttonSets[i].SetActive(i == activeSet);
        }
    }
}