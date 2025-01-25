using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GamePlay;
using UnityEngine.InputSystem;

public class mil : MonoBehaviour, IInteract
{
    // Start is called before the first frame update

    Sequence MilSeq;
    Sequence downSequence;

    public bool isEnd;

    private Vector3 Base_Pos;
    private Vector3 origineAngle;

    public void Awake()
    {
        Base_Pos = transform.localPosition;
        origineAngle = transform.localEulerAngles;
    }

    private void Start()
    {
        init_mil();
        shake_mil();
    }
    public void init_mil()
    {
        isEnd = false;
        downSequence.Kill();
        transform.localPosition = Base_Pos;
        transform.localRotation = Quaternion.identity;
    }

    public void shake_mil()
    {
        MilSeq = DOTween.Sequence();
        MilSeq.Append(transform.DOLocalMove(new Vector3(0, -0.025f, 0.08f), 3f).SetRelative(true).SetEase(Ease.OutQuad));
        MilSeq.Join(transform.DOLocalRotate(new Vector3(40, 0, 0f), 4f).SetRelative(true).SetEase(Ease.OutQuad));
        MilSeq.OnComplete(() =>
        {
            isEnd = true;
            downSequence = DOTween.Sequence();
            downSequence.Append(transform.DOLocalMove(new Vector3(0, -1.2f, 0.5f), 3f).SetRelative(true).SetEase(Ease.InSine));
            downSequence.Join(transform.DOLocalRotate(new Vector3(145, 0, 0f), 2f).SetRelative(true).SetEase(Ease.InSine));
            downSequence.OnComplete(init_mil);
        });
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            init_mil();
        }
    }
}
