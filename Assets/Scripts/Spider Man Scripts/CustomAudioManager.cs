using UnityEngine;

public class CustomAudioManager : MonoBehaviour
{
    public AudioClip backgroundMusicClip;
    public bool playBackgroundMusic = true;

    private AudioSource backgroundMusicSource;

    private void Start()
    {
        // Create an AudioSource for the background music
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusicClip;
        backgroundMusicSource.loop = true;

        // Play the background music if it is enabled
        if (playBackgroundMusic)
        {
            PlayBackgroundMusic();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void ToggleBackgroundMusic()
    {
        playBackgroundMusic = !playBackgroundMusic;

        if (playBackgroundMusic)
        {
            PlayBackgroundMusic();
        }
        else
        {
            StopBackgroundMusic();
        }
    }
}
