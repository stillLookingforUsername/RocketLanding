using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class Lander : MonoBehaviour
{
    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            _rb.AddForce(force * transform.up * Time.deltaTime); //we don't need deltaTime in fixedUpdate but just for unexpected error used it
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = 200f;
            _rb.AddTorque(turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -200f;
            _rb.AddTorque(turnSpeed * Time.deltaTime);
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

}