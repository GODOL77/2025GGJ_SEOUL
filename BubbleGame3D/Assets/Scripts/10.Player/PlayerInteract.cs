using System;
using GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    // 플레이어 상호작용
    public class PlayerInteract : MonoBehaviour
    {
        private void Awake()
        {
            InputManager.Interact.started += ClickEvent;
            InputManager.Interact.performed += ClickEvent;
            InputManager.Interact.canceled += ClickEvent;
        }

        public void OnDestroy()
        {
            if(!InputManager.HasInstance) return;
            InputManager.Interact.started -= ClickEvent;
            InputManager.Interact.performed -= ClickEvent;
            InputManager.Interact.canceled -= ClickEvent;
        }

        public void ClickEvent(InputAction.CallbackContext context)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                foreach (var interact in hit.transform.gameObject.GetComponentsInChildren<IInteract>())
                {
                    interact.Interact(context);
                }
                Debug.Log($"플레이어가 {hit.transform.name}과 상호작용");
            }
        }
    }
}