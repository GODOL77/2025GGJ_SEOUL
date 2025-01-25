using GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;

public class GasLever : MonoBehaviour, IInteract
{
    [SerializeField] private Gasburner Burner;
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Burner.InteractLever();

        }
    }
}
