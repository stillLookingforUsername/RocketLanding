using System;
using TMPro;
using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    //this scripts is only for visual,i didn't use logic here so this script needs to be manipulate using Events
    //this class will be notified when i press sth
    [SerializeField] private ParticleSystem LeftThrusterParticles;
    [SerializeField] private ParticleSystem MiddleThrusterParticles;
    [SerializeField] private ParticleSystem RightThrusterParticles;

    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        //all should be off at the game first start
        SetEnableThrusterParticleSystem(LeftThrusterParticles, false);
        SetEnableThrusterParticleSystem(MiddleThrusterParticles, false);
        SetEnableThrusterParticleSystem(RightThrusterParticles, false);
    }

    //this method is used to turn off particles every FixedUpdate time
    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(LeftThrusterParticles, false);
        SetEnableThrusterParticleSystem(MiddleThrusterParticles, false);
        SetEnableThrusterParticleSystem(RightThrusterParticles, false);
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(RightThrusterParticles, true);
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(LeftThrusterParticles, true);
    }


    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(LeftThrusterParticles, true);
        SetEnableThrusterParticleSystem(MiddleThrusterParticles, true);
        SetEnableThrusterParticleSystem(RightThrusterParticles, true);
    }

    private void SetEnableThrusterParticleSystem(ParticleSystem particleSystem,bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule =  particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}