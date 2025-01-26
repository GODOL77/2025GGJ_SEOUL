using System;
using GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fragment : MonoBehaviour, IInteract
{
    public void Awake()
    {
        Destroy(gameObject, 7f);
    }

    public void OnCollisionEnter(Collision other)
    {
        if ((LayerMask.GetMask("Bubble Attacker") & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Destroy(gameObject);
        }
    }
}
