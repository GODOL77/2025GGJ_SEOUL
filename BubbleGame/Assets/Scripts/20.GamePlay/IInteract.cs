using UnityEngine.InputSystem;

namespace GamePlay
{
    public interface IInteract
    {
        public void Interact(InputAction.CallbackContext context);
    }
}