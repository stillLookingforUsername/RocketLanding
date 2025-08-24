using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _fuelClip;
    [SerializeField] private AudioClip _coinClip;
    [SerializeField] private AudioClip _successClip;
    [SerializeField] private AudioClip _crashClip;
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
                AudioSource.PlayClipAtPoint(_successClip, Camera.main.transform.position);
                break;
            default:
                AudioSource.PlayClipAtPoint(_crashClip,Camera.main.transform.position);
                break;
        }
    }

    private void Lander_OnFuelPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(_fuelClip,Camera.main.transform.position);
    }

    private void Lander_OnCoinPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(_coinClip,Camera.main.transform.position);
    }
}