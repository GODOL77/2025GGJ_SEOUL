using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class InputManager : Singleton<InputManager>
{
    public static Vector2 MovePosition => Instance.input.PlayerAction.Move.ReadValue<Vector2>();

    public static InputAction CameraMoveLeft => Instance.input.Camera.MoveLeft;
    public static InputAction CameraMoveRight => Instance.input.Camera.MoveRight;
    public static bool CameraMoveLeftPress => Instance.input.Camera.MoveLeft.ReadValue<float>() > 0f;
    public static bool CameraMoveRightPress => Instance.input.Camera.MoveRight.ReadValue<float>() > 0f;
    
    public static Vector2 MousePosition => Mouse.current.position.value;

    public static InputAction Interact => Instance.input.PlayerAction.Interact;

    private PlayerInputSystem input;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        input = new();
        input.Enable();
    }
}
