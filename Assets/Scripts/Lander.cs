using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    public static Lander Instance { get; private set; }
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickUp;
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector; //for angle
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }
    //First we need some enum to define all the State
    public enum State
    {
        WaitingToStart,
        Normal,
    }
    private Rigidbody2D _rb;

    private float fuelAmount = 10f;
    private float fuelAmountMax = 10f;
    private State state; //field for our currentState
    private void Awake()
    {
        Instance = this;
        fuelAmount = fuelAmountMax;
        //default State to WaitingToStart
        state = State.WaitingToStart;
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;

    }
    //An interesting gameDesign question - do u want the fuel to be consumtion to be double(like it will be decremented twice when u press 2 buttons like up and left at the same time)
    //Or u want the fuel Consumption to be at a specific rate no matter how many buttons u press(like even if u press up and left at the same time it will consume only 1f(1 ltr) at a time)
    private void FixedUpdate()
    {

        Debug.Log("FuelAmount: " + fuelAmount);
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    //press anyInput
                    _rb.gravityScale = GRAVITY_NORMAL;
                    state = State.Normal;
                }
                break;
            case State.Normal:
                    if (fuelAmount <= 0f)
                    {
                        //No fuel
                        return;
                    }
                    if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                    {
                        //press anyInput
                        FuelConsumption();
                    }
                    if (Keyboard.current.upArrowKey.isPressed)
                    {
                        float force = 700f;
                        _rb.AddForce(force * transform.up * Time.deltaTime); //we don't need deltaTime in fixedUpdate but just for unexpected error used it
                        OnUpForce?.Invoke(this, EventArgs.Empty);
                    }
                    if (Keyboard.current.leftArrowKey.isPressed)
                    {
                        float turnSpeed = 200f;
                        _rb.AddTorque(turnSpeed * Time.deltaTime);
                        OnLeftForce?.Invoke(this, EventArgs.Empty);
                    }
                    if (Keyboard.current.rightArrowKey.isPressed)
                    {
                        float turnSpeed = -200f;
                        _rb.AddTorque(turnSpeed * Time.deltaTime);
                        OnRightForce?.Invoke(this, EventArgs.Empty);
                    }
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crash Land");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0,
            });
            return;
        }
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = other.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            //landed too hard
            Debug.Log("Landed too hard");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = landingPad.GetScoreMultiplier(),
                score = 0,
            });
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
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = landingPad.GetScoreMultiplier(),
                score = 0,
            });
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
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out FuelPickUp fuelPickup))
        {
            float addFuelAmt = 10f;
            fuelAmount += addFuelAmt;
            //dont destroy the other fuelGameObejct here - the specific gameObect should be responsible for destroying itself
            //clean Code method - define destroySelf function in that gameObject's script and call it from here
            if (fuelAmount > fuelAmountMax)
            {
                //fuel will go up only till max it won't keep on incrementing
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (other.gameObject.TryGetComponent(out CoinPickUp coinPickUp))
        {
            OnCoinPickUp?.Invoke(this, EventArgs.Empty);
            coinPickUp.DestroySelf();
        }

    }

    //Optional - use parameter to dictate how much fuel to be consumed
    //private void FuelConsumption(int consumptionAmt)
    private void FuelConsumption()
    {
        float fuelConsumptionAmt = 1f;
        fuelAmount -= fuelConsumptionAmt * Time.deltaTime;   //avoid using magic numbers
    }

    public float GetSpeedX()
    {
        return _rb.linearVelocityX;
    }
    public float GetSpeedY()
    {
        return _rb.linearVelocityY;
    }
    public float GetFuelAmountNormalize()
    {
        return fuelAmount / fuelAmountMax;
    }
    public float GetFuel()
    {
        return fuelAmount;
    }
}