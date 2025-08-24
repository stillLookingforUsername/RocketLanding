using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private const int SOUND_VOLUME_MAX = 10;
    public event EventHandler OnSoundVolumeChanged;
    private static int soundVolume = 5; //static so that it persist through scene
    [SerializeField] private AudioClip _fuelClip;
    [SerializeField] private AudioClip _coinClip;
    [SerializeField] private AudioClip _successClip;
    [SerializeField] private AudioClip _crashClip;

    private void Awake()
    {
        Instance = this;
    } 
    private void Start()
    {
        Lander.Instance.OnCoinPickUp += Lander_OnCoinPickUp;
        Lander.Instance.OnFuelPickUp += Lander_OnFuelPickUp;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                AudioSource.PlayClipAtPoint(_successClip, Camera.main.transform.position,GetSoundVolumeNormalized());
                break;
            default:
                AudioSource.PlayClipAtPoint(_crashClip, Camera.main.transform.position,GetSoundVolumeNormalized());
                break;
        }
    }

    private void Lander_OnFuelPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(_fuelClip, Camera.main.transform.position,GetSoundVolumeNormalized());
    }

    private void Lander_OnCoinPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(_coinClip, Camera.main.transform.position,GetSoundVolumeNormalized());
    }
    public void ChangeSound()
    {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX; //it basically loop back once it reaches 10
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetSoundVolume()
    {
        return soundVolume;
    }
    public float GetSoundVolumeNormalized()
    {
        return ((float)soundVolume) / SOUND_VOLUME_MAX;
    }

}