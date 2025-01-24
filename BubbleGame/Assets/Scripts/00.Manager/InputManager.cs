using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class InputManager : Singleton<InputManager>
{
    public static Vector2 MovePosition => Instance.input.PlayerAction.Move.ReadValue<Vector2>();
    
    public static Vector2 MousePosition => Mouse.current.position.value;

    private PlayerInputSystem input;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        input = new();
        input.Enable();
    }
}
