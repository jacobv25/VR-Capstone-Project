using System.Collections.Generic;
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
            triggerButtonLeft.ClearGlow();
            gripButtonLeft.SetGlow();
            StartCoroutine(PlayLeftGripTutorial());
        }
        else if (currentStep == 2 && InputBridge.Instance.LeftGripDown) //completed grip tutorial
        {
            gripButtonLeft.ClearGlow();
            trackpadButtonLeft.SetGlow();
            StartCoroutine(PlayLeftTrackpadTutorial());
        }
        else if (currentStep == 3 && InputBridge.Instance.LeftThumbstickDown) //completed trackpad tutorial
        {
            trackpadButtonLeft.ClearGlow();

        }
    }

    private IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlayAudioClip(introAudio);
        float duration = introAudio.length;
        yield return new WaitForSeconds(duration);
        StartCoroutine(PlayLeftTriggerTutorial());
    }

    private IEnumerator PlayLeftTriggerTutorial()
    {
        triggerButtonLeft.SetGlow();
        AudioManager.Instance.PlayAudioClip(triggerAudioLeft);
        float duration = triggerAudioLeft.length;       
        yield return new WaitForSeconds(duration);
        currentStep = 1;
    }
    private IEnumerator PlayLeftGripTutorial()
    {
        AudioManager.Instance.PlayAudioClip(gripAudioLeft);
        float duration = gripAudioLeft.length;
        yield return new WaitForSeconds(duration);
        currentStep = 2;
    }

    private IEnumerator PlayLeftTrackpadTutorial()
    {
        AudioManager.Instance.PlayAudioClip(trackpadAudioLeft);
        float duration = trackpadAudioLeft.length;
        yield return new WaitForSeconds(duration);
        currentStep = 3;
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
