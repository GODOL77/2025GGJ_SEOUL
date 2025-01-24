using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class InputManager : Singleton<InputManager>
{
    public static Vector2 MousePosition => Mouse.current.position.value;
}
