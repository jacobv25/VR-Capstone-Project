using UnityEngine;
using BNG;
using System.Collections;

public class TutorialController : MonoBehaviour
{

    [SerializeField] private AudioClip tutorialIntroAudio;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject cube1;
    [SerializeField] private GameObject cube2;
    [SerializeField] private GameObject cube3;



    [Header("Audio Clips")]
    [SerializeField] private AudioClip triggerAudio;
    [SerializeField] private AudioClip gripAudio;
    [SerializeField] private AudioClip trackpadAudio;
    [SerializeField] private AudioClip virtualHandsIntroAudio;
    [SerializeField] private AudioClip virtualHandsTutorialAudio;
    [SerializeField] private AudioClip pickUpObjectAudio;
    [SerializeField] private AudioClip playWithItemsAudio;
    [SerializeField] private AudioClip navigationIntroAudio;


    [Header("SFX")]
    [SerializeField] private AudioClip buttonPressSFX;

    [Header("Left Button References")]
    [SerializeField] private ButtonController trackpadButtonLeft;
    [SerializeField] private ButtonController triggerButtonLeft;
    [SerializeField] private ButtonController gripButtonLeft;

    [Header("Right Button References")]
    [SerializeField] private ButtonController trackpadButtonRight;
    [SerializeField] private ButtonController triggerButtonRight;
    [SerializeField] private ButtonController gripButtonRight;

    [Header("Particle Effect")]
    [SerializeField] private ParticleSystem particleSystem;

    [Header("Hand Model Selector")]
    [SerializeField] private HandModelSelector handModelSelector;
    
    [Header("Debugging")]
    public bool debug;
    [HideInInspector] public int currentStep = 0;


    private int controllerModelIndex = 0;
    private int handModelIndex = 1;


    private bool leftButtonPressed = false;
    private bool rightButtonPressed = false;

    private SceneLoader sceneLoader;

    private void OnEnable()
    {
        InputBridge.OnInputsUpdated += CheckInputs;
    }

    private void OnDisable()
    {
        InputBridge.OnInputsUpdated -= CheckInputs;
    }

    private void Start()
    {
        if(!debug)
        { 
            StartCoroutine(PlayIntro());
        }
        sceneLoader = GetComponent<SceneLoader>();
    }

    private void CheckInputs()
    {
        if (currentStep == 1)
        {
            if (InputBridge.Instance.LeftTriggerDown)
            {
                SetButtonGlow(triggerButtonLeft, false);
                ShowParticleEffect(triggerButtonLeft);
                PlaySFX();
                leftButtonPressed = true;
            }
            if(InputBridge.Instance.RightTriggerDown)
            {
                SetButtonGlow(triggerButtonRight, false);
                ShowParticleEffect(triggerButtonRight);
                PlaySFX();
                rightButtonPressed = true;
            }
            if(leftButtonPressed && rightButtonPressed)
            {
                PlayTutorial(gripAudio, 2, gripButtonLeft, gripButtonRight);
            }
        }
        else if (currentStep == 2) 
        {
            if(InputBridge.Instance.LeftGripDown)
            {
                ShowParticleEffect(gripButtonLeft);
                PlaySFX();
                SetButtonGlow(gripButtonLeft, false);
                leftButtonPressed = true;
            }
            if (InputBridge.Instance.RightGripDown)
            {
                ShowParticleEffect(gripButtonRight);
                PlaySFX();
                SetButtonGlow(gripButtonRight, false);
                rightButtonPressed = true;
            }
            if (leftButtonPressed && rightButtonPressed)
            {
                PlayTutorial(trackpadAudio, 3, trackpadButtonLeft, trackpadButtonRight);
            }
        }
        else if (currentStep == 3) 
        {
            if (InputBridge.Instance.LeftThumbstickDown)
            {
                ShowParticleEffect(trackpadButtonLeft);
                PlaySFX();
                SetButtonGlow(trackpadButtonLeft, false);
                leftButtonPressed = true;
            }
            if (InputBridge.Instance.RightThumbstickDown)
            {
                ShowParticleEffect(trackpadButtonRight);
                PlaySFX();
                SetButtonGlow(trackpadButtonRight, false);
                rightButtonPressed = true;
            }
            if (leftButtonPressed && rightButtonPressed)
            {
                leftButtonPressed = false;
                rightButtonPressed = false;
                currentStep = 4;
                StartCoroutine(VirtualHandsTutorial());

            }
        }
        else if ( currentStep == 4)
        {
            //do grabbing demo. BLOCKS, PAPER AIR PLANE, PING PONG PADDLE
        }
    }

    private IEnumerator VirtualHandsTutorial()
    {
        AudioManager.Instance.PlayAudioClip(virtualHandsIntroAudio);
        float duration = tutorialIntroAudio.length;
        yield return new WaitForSeconds(3);
        handModelSelector.ChangeHandsModel(handModelIndex);
        yield return new WaitForSeconds(1);
        if (!debug)
        {
            AudioManager.Instance.PlayAudioClip(virtualHandsTutorialAudio);
            yield return new WaitForSeconds(11);
        }
        //show red button and wait for press
        button.SetActive(true);
        button.GetComponent<RaiseAndLowerObject>().Raise();
    }

    public void PressButton()
    {
        //play audio
        AudioManager.Instance.PlayAudioClip(pickUpObjectAudio);
        //lower button
        button.GetComponent<RaiseAndLowerObject>().LowerAndDeactivate();
        //make items appear
        cube1.SetActive(true);
        cube2.SetActive(true);
        cube3.SetActive(true);
        // raise them
        cube1.GetComponent<RaiseAndLowerObject>().Raise();
        cube2.GetComponent<RaiseAndLowerObject>().Raise();
        cube3.GetComponent<RaiseAndLowerObject>().Raise();

        StartCoroutine(SceneTransition());
    }

    private IEnumerator SceneTransition()
    {
        if (!debug)
        {
            yield return new WaitForSeconds(20);
        }
        //transition to walkng tutorial
        AudioManager.Instance.PlayAudioClip(navigationIntroAudio);
        yield return new WaitForSeconds(4);

        sceneLoader.LoadScene("NavigationTutorialScene");
    }

    private IEnumerator PlayWithItems()
    {
        yield return new WaitForSeconds(3);
        AudioManager.Instance.PlayAudioClip(playWithItemsAudio);
        //raise new items to play with
    }

    private IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlayAudioClip(tutorialIntroAudio);
        float duration = tutorialIntroAudio.length;
        yield return new WaitForSeconds(duration + 2);
        PlayTutorial(triggerAudio, 1, triggerButtonLeft, triggerButtonRight);
    }

    private void PlayTutorial(AudioClip audioClip, int nextStep, ButtonController leftButtonToGlow, ButtonController rightButtonToGlow)
    {
        leftButtonPressed = false;
        rightButtonPressed = false;
         
        SetButtonGlow(leftButtonToGlow, true);
        SetButtonGlow(rightButtonToGlow, true);

        AudioManager.Instance.StopPlayingAudio();

        AudioManager.Instance.PlayAudioClip(audioClip);

        currentStep = nextStep;
    }

    private void SetButtonGlow(ButtonController button, bool isGlowing)
    {
        if (isGlowing)
        {
            button.SetGlow();
        }
        else
        {
            button.ClearGlow();
        }
    }

    // Show particle effect on the specified button
    public void ShowParticleEffect(ButtonController button)
    {
        // Set the particle effect's position to the button's position
        particleSystem.transform.position = button.transform.position;

        // Stop the particle effect in case it's already playing
        particleSystem.Stop();

        // Play the particle effect
        particleSystem.Play();
    }

    private void PlaySFX()
    {
        SFXManager.Instance.PlaySFX(buttonPressSFX);
    }

    public void WalkingTutorialComplete()
    {
        Debug.Log("Waklking tutorial complete!");
    }
}


