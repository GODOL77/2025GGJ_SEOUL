using GamePlay;
using Gimmick;
using UnityEngine;
using UnityEngine.InputSystem;

public class GasLever : MonoBehaviour, IInteract
{
    [SerializeField] private Gasburner Burner;
    public GimmickMaterialControl gimmickMaterialControl;
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Burner.InteractLever();

        }
    }
}
