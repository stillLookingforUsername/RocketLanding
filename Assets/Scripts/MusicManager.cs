using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //the reason we use this script is cuz if we directly use AudioSource to play the bg music it restart and play again when new scene
    //is loaded
    //this script will play the music continously without break even if new scene is loaded

    public static MusicManager Instance { get; private set; }
    private const int MUSIC_VOLUME_MAX = 10;
    private static float musicTime;
    private static int musicVolume = 4;
    private AudioSource musicAudioSource;
    private event EventHandler OnMusicVolumeChanged;

    private void Awake()
    {
        Instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
    }
    private void Start()
    {
        musicAudioSource.volume = GetMusicVolumeNormalized();
    }
    private void Update()
    {
        musicTime = musicAudioSource.time;
    }
    public void ChangeMusic()
    {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX; //it basically loop back once it reaches 10
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetMusicVolumeNormalized()
    {
        return ((float)musicVolume) / MUSIC_VOLUME_MAX;
    }

}