using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Audio clip is null");
        }
    }
}
