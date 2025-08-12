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
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(0, 1))); //same direction
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(0.5f,0.5f))); //45 degree
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(1,0)));    //90 degree
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(0, -1)));   //opposite direction
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
        float softLandingVelocityMagnitude = 4f;
        if (other.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            //landed too hard
            Debug.Log("Crash");
            return;
        }
        else { Debug.Log("Soft Landing"); }
    }

}