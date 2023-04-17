using UnityEngine;
using BNG;
using System.Collections;

public class TutorialController : MonoBehaviour
{

    [SerializeField] private AudioClip introAudio;

    [Header("Audio Clips Left Controller")]
    [SerializeField] private AudioClip trackpadAudioLeft;
    [SerializeField] private AudioClip triggerAudioLeft;
    [SerializeField] private AudioClip gripAudioLeft;

    [Header("Audio Clips Right Controller")]
    [SerializeField] private AudioClip introAudioRight;
    [SerializeField] private AudioClip trackpadAudioRight;
    [SerializeField] private AudioClip triggerAudioRight;
    [SerializeField] private AudioClip gripAudioRight;

    [Header("Particle Effect")]
    [SerializeField] private ParticleSystem particleSystem;

    [Header("Left Button References")]
    [SerializeField] private ButtonController trackpadButtonLeft;
    [SerializeField] private ButtonController triggerButtonLeft;
    [SerializeField] private ButtonController gripButtonLeft;

    [Header("Right Button References")]
    [SerializeField] private ButtonController trackpadButtonRight;
    [SerializeField] private ButtonController triggerButtonRight;
    [SerializeField] private ButtonController gripButtonRight;

    private int currentStep = 0;

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
        StartCoroutine(PlayIntro());
    }

    private void CheckInputs()
    {
        if (currentStep == 1 && InputBridge.Instance.LeftTriggerDown) //completed trigger tutorial
        {
            SetButtonGlow(triggerButtonLeft, false);
            SetButtonGlow(gripButtonLeft, true);
            StartCoroutine(PlayTutorial(gripAudioLeft, 2));
        }
        else if (currentStep == 2 && InputBridge.Instance.LeftGripDown) //completed grip tutorial
        {
            SetButtonGlow(gripButtonLeft, false);
            SetButtonGlow(trackpadButtonLeft, true);
            StartCoroutine(PlayTutorial(trackpadAudioLeft, 3));
        }
        else if (currentStep == 3 && InputBridge.Instance.LeftThumbstickDown) //completed trackpad tutorial
        {
            SetButtonGlow(trackpadButtonLeft, false);
        }
    }

    private IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlayAudioClip(introAudio);
        float duration = introAudio.length;
        yield return new WaitForSeconds(duration);
        StartCoroutine(PlayTutorial(triggerAudioLeft, 1, triggerButtonLeft));
    }

    private IEnumerator PlayTutorial(AudioClip audioClip, int nextStep, ButtonController buttonToGlow = null)
    {
        if (buttonToGlow != null)
        {
            SetButtonGlow(buttonToGlow, true);
        }
        AudioManager.Instance.PlayAudioClip(audioClip);
        float duration = audioClip.length;
        yield return new WaitForSeconds(duration);
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
}


