using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CustomVRPauseMenu : MonoBehaviour
{

    // Reference to the CustomLocomotionManager component
    [SerializeField] private LocomotionManager locomotionManager;
    [SerializeField] private PlayerRotation playerRotation;
    [SerializeField] private GameObject pauseMenu; // The pause menu UI

    private void OnEnable()
    {
        InputBridge.OnInputsUpdated += CheckInputs;
    }

    private void OnDisable()
    {
        InputBridge.OnInputsUpdated -= CheckInputs;
    }
    
    void Start() {
        // Get the CustomLocomotionManager component from the scene if it's not assigned
        if(locomotionManager == null) {
            Debug.LogError("LocomotionManager not assigned. Searching scene for CustomLocomotionManager component.");
        }
    }

private float lastRightTriggerPressTime = 0f;
private int rightTriggerPressCount = 0;
[SerializeField] private float rightTriggerPressInterval = 1f; //second

    private void CheckInputs()
    {
        if (InputBridge.Instance.RightTriggerDown)
        {
            Debug.Log("Right trigger pressed!");
            float currentTime = Time.time;
            float timeSinceLastPress = currentTime - lastRightTriggerPressTime;

            if (timeSinceLastPress <= rightTriggerPressInterval)
            {
                rightTriggerPressCount++;

                if (rightTriggerPressCount == 3)
                {
                    // Show pause menu
                    Debug.Log("Pause menu triggered!");
                    ShowPauseMenu();
                    rightTriggerPressCount = 0;
                }
            }
            else
            {
                rightTriggerPressCount = 1;
            }

            lastRightTriggerPressTime = currentTime;
        }
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            ShowPauseMenu();
        }    
    }

    private void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void UseSmoothLocomotionMovement()
    {
        locomotionManager.ChangeLocomotion(LocomotionType.SmoothLocomotion, locomotionManager.LoadLocomotionFromPrefs);
    }

    public void UseTeleportMovement()
    {
        locomotionManager.ChangeLocomotion(LocomotionType.Teleport, locomotionManager.LoadLocomotionFromPrefs);
    }

    public void UseSmoothTurn()
    {
        playerRotation.AllowInput = true;
        playerRotation.RotationType = RotationMechanic.Smooth;
    }

    public void UseSnapTurn()
    {
        playerRotation.AllowInput = true;
        playerRotation.RotationType = RotationMechanic.Snap;
    }

    public void DisableTurn()
    {
        playerRotation.AllowInput = false;
    }

}
