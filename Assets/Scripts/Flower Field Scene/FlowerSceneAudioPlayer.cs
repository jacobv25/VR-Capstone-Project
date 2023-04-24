using System.Collections;
using UnityEngine;

public class FlowerSceneAudioPlayer : MonoBehaviour
{
    public AudioClip firstClip;
    public AudioClip secondClip;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    [Range(0f, 1f)] public float volumeSource1 = 1f;
    [Range(0f, 1f)] public float volumeSource2 = 1f;

    private void Start()
    {
        StartCoroutine(PlayAudioClips());
    }

    private void Update()
    {
        UpdateAudioVolumes();
    }

    private IEnumerator PlayAudioClips()
    {
        audioSource1.clip = firstClip;
        audioSource1.Play();

        yield return new WaitForSeconds(12f); // Wait for 12 seconds before playing the second clip

        audioSource2.clip = secondClip;
        audioSource2.Play();
    }

    private void UpdateAudioVolumes()
    {
        audioSource1.volume = volumeSource1;
        audioSource2.volume = volumeSource2;
    }
}
