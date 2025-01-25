using GamePlay;
using Gimmick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlourBag : MonoBehaviour, IInteract
{
    // Start is called before the first frame update
    public mil scr;
    public ParticleSystem FlourSmog;
    public GimmickMaterialControl gimmickMaterialControl;

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        
        }
    }
}
