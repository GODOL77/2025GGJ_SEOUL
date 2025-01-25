﻿using GamePlay;
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

        public void ClickEvent(InputAction.CallbackContext context)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue) &&
                hit.transform.gameObject.TryGetComponent(out IInteract interact))
            {
                interact.Interact(context);
            }
        }
    }
}