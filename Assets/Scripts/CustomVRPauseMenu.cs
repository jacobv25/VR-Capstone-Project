using UnityEngine.UI;
using UnityEngine;
using BNG;
using UnityEngine.EventSystems;
using System;

public class CustomVRPauseMenu : MonoBehaviour
{

    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color unselectedColor = Color.white;


    // Reference to the CustomLocomotionManager component
    [SerializeField] private LocomotionManager locomotionManager;
    [SerializeField] private PlayerRotation playerRotation;

    [SerializeField] private GameObject pauseMenu; // Assign your pause menu prefab in the Unity Inspector
    [SerializeField] private float distanceFromPlayer = 5.0f; // Set the distance from the player where the pause menu should appear

    [Header("Normal Pause Menu")]
    [SerializeField] private GameObject snapButton;
    [SerializeField] private GameObject noneButton;
    [SerializeField] private GameObject smoothButton;
    [SerializeField] private GameObject teleportButton;

    [Header("For Special Scenes (Racing, Flower, etc)")]
    [SerializeField] private bool useSpecialMenu = false;

    [Header("Detecting Trigger Pull")]
    public int triggerPullCount = 0;
    public float triggerPullInterval = 0.3f; // The time window for 5 trigger pulls
    private float triggerPullTimer = 0.0f;
    //private bool isTriggerPulled = false;


    private void OnEnable()
    {
        InputBridge.OnInputsUpdated += CheckInputs;
    }

    private void OnDisable()
    {
        InputBridge.OnInputsUpdated -= CheckInputs;
    }

    private void CheckInputs()
    {
        if (InputBridge.Instance.RightTriggerDown) 
        {
            triggerPullCount++;
            triggerPullTimer = triggerPullInterval;
        }


        // Update the trigger pull timer
        if (triggerPullTimer > 0.0f)
        {
            triggerPullTimer -= Time.deltaTime;
        }
        else
        {
            triggerPullCount = 0;
        }

        // Check if the right trigger has been pulled 5 times in a row quickly (within 1.5 seconds)
        if (triggerPullCount >= 5)
        {
            if(useSpecialMenu)
            {
                DisplaySpecialPauseMenu();
            }
            else 
            {
                DisplayPauseMenu();
            }
            triggerPullCount = 0; // Reset the counter
        }
    }

    public void DisplaySpecialPauseMenu()
    {
        if(pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            // Calculate the position where the pause menu should be displayed
            Vector3 menuPosition = transform.position + transform.forward * distanceFromPlayer;
            // Add a vertical offset to the menu position
            float verticalOffset = 0.5f; // Change this value to your desired offset
            menuPosition += new Vector3(0, verticalOffset, 0);
            // Set the position of the pause menu
            pauseMenu.transform.position = menuPosition;
        }
    }

    public void DisplayPauseMenu()
    {
        // Activate the pause menu if it exists
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);

            // Calculate the position where the pause menu should be displayed
            Vector3 menuPosition = transform.position + transform.forward * distanceFromPlayer;

            // Add a vertical offset to the menu position
            float verticalOffset = 0.5f; // Change this value to your desired offset
            menuPosition += new Vector3(0, verticalOffset, 0);

            // Set the position of the pause menu
            pauseMenu.transform.position = menuPosition;

            // Update button colors based on the current settings
            UpdateButtonColors();
        }
    }

    private void UpdateButtonColors()
    {
        // Update locomotion buttons
        if (locomotionManager.SelectedLocomotion == LocomotionType.SmoothLocomotion)
        {
            smoothButton.GetComponent<Image>().color = selectedColor;
            teleportButton.GetComponent<Image>().color = unselectedColor;
        }
        else if (locomotionManager.SelectedLocomotion == LocomotionType.Teleport)
        {
            smoothButton.GetComponent<Image>().color = unselectedColor;
            teleportButton.GetComponent<Image>().color = selectedColor;
        }

        // Update rotation buttons
        if (playerRotation.RotationType == RotationMechanic.Snap && playerRotation.AllowInput)
        {
            snapButton.GetComponent<Image>().color = selectedColor;
            noneButton.GetComponent<Image>().color = unselectedColor;
        }
        else
        {
            snapButton.GetComponent<Image>().color = unselectedColor;
            noneButton.GetComponent<Image>().color = selectedColor;
        }
    }


    public void UseSmoothLocomotionMovement()
    {
        locomotionManager.ChangeLocomotion(LocomotionType.SmoothLocomotion, locomotionManager.LoadLocomotionFromPrefs);
        UpdateButtonColors();
    }

    public void UseTeleportMovement()
    {
        locomotionManager.ChangeLocomotion(LocomotionType.Teleport, locomotionManager.LoadLocomotionFromPrefs);
        UpdateButtonColors();
    }

    public void UseSnapTurn()
    {
        playerRotation.AllowInput = true;
        playerRotation.RotationType = RotationMechanic.Snap;
        UpdateButtonColors();

    }

    public void DisableTurn()
    {
        playerRotation.AllowInput = false;
        UpdateButtonColors();
    }

}
