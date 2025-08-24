using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;

    private Lander _lander;
    private void Awake()
    {
        _lander = GetComponent<Lander>();
    }
    private void Start()
    {
        _lander.OnBeforeForce += Lander_OnBeforeForce;
        _lander.OnUpForce += Lander_OnUpForce;
        _lander.OnLeftForce += Lander_OnLeftForce;
        _lander.OnRightForce += Lander_OnRightForce;
        SoundManager.Instance.OnSoundVolumeChanged += SoudnManager_OnSoundVolumeChanged;

        thrusterAudioSource.Pause();
    }

    private void SoudnManager_OnSoundVolumeChanged(object sender, EventArgs e)
    {
        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        thrusterAudioSource.Pause();
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        thrusterAudioSource.Play();
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        thrusterAudioSource.Play();
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        thrusterAudioSource.Pause();
    }
}
