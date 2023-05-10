using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioClip backgroundSong;
    public bool playOnStart = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playOnStart && audioSource != null)
        {
            audioSource.PlayOneShot(backgroundSong);
        }
    }
}
