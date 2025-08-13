using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Lander : MonoBehaviour
{
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    private Rigidbody2D _rb;

    private float fuelAmount = 10f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            _rb.AddForce(force * transform.up * Time.deltaTime); //we don't need deltaTime in fixedUpdate but just for unexpected error used it
            FuelConsumption(); //can use parameter to add how much fuel to consume while using different movement
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = 200f;
            _rb.AddTorque(turnSpeed * Time.deltaTime);
            FuelConsumption();
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -200f;
            _rb.AddTorque(turnSpeed * Time.deltaTime);
            FuelConsumption();
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crash Land");
            return;
        }
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = other.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            //landed too hard
            Debug.Log("Crash");
            return;
        }
        //we are going to compare the dot product of Global Vector & Local Vector(of the gameObject).
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        //Debug.Log(dotVector);
        float minDotVector = 0.9f;
        if (dotVector < minDotVector)
        {
            //landed on a steep angle
            Debug.Log("Landing Angle is too steep");
            return;
        }
        Debug.Log("Soft Landing");

        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMltiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMltiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("LandingAngleScore" + landingAngleScore);
        Debug.Log("LandingSpeedScore" + landingSpeedScore);

        int score = Mathf.RoundToInt(landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier();
        Debug.Log("Score: " + score);
    }

    private void FuelConsumption()
    {
        float fuelConsumptionAmt = 1f;
        fuelAmount -= fuelConsumptionAmt * Time.deltaTime;   //avoid using magic numbers
    }

}