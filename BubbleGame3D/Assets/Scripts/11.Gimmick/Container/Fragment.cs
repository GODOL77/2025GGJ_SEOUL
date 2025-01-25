using GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fragment : MonoBehaviour, IInteract
{
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Destroy(gameObject);
        }
    }
}
