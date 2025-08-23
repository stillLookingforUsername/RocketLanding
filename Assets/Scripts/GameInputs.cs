using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public static GameInputs Instance { get; private set; }
    private InputActions _inputActions;

    private void Awake()
    {
        Instance = this;
        _inputActions = new InputActions();
        _inputActions.Enable(); //we must need to enable the inputActions otherwise it's won't work
    }
    public void OnDestroy()
    {
        _inputActions.Disable();
    }
    public bool IsUpActionPressed()
    {
        return _inputActions.Player.LanderUp.IsPressed();
    }
    public bool IsLeftActionPressed()
    {
        return _inputActions.Player.LanderLeft.IsPressed();
    }
    public bool IsRightActionPressed()
    {
        return _inputActions.Player.LanderRight.IsPressed();
    }
    public Vector2 GetMovementInputVector2()
    {
        return _inputActions.Player.Movement.ReadValue<Vector2>();
    }

}
