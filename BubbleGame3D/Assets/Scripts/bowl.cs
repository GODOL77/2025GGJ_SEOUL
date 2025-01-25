using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using GamePlay;
using Gimmick;
using UnityEngine.InputSystem;

public class bowl : MonoBehaviour, IInteract
{
    // Start is called before the first frame update

    public GameObject bowls;
    public float intervalCount = 2;
    public float delayDuration = 1f;
    public float speed = 0.6f;
    
    public GimmickSequence gimmickSequence;

    private Sequence downSequence;
    private Collider _collider;
    private Vector3 originPosition;
    private Vector3 originAngle;
    private bool isShakeEnd;
    public bool IsShakeEnd
    {
        get => isShakeEnd;
        private set => isShakeEnd = value;
    }

    public void Awake()
    {
        _collider = GetComponent<Collider>();
        originPosition = gameObject.transform.localPosition;
        originAngle = gameObject.transform.localEulerAngles;
        IsShakeEnd = false;
    }

    public void Init_bowl()
    {
        downSequence.Kill();
        IsShakeEnd = false;
        gameObject.transform.localPosition = originPosition;
        gameObject.transform.localEulerAngles = originAngle;
    }

    public void Shake_bowl()
    {
        var BowlSeq = DOTween.Sequence();
        for (int i = 0; i < intervalCount; i++)
        {
            BowlSeq.Append(transform.DOLocalRotate(new Vector3(0, 0, 8), speed).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(delayDuration));
            BowlSeq.Join(transform.DOLocalMoveZ(0.01f, speed + 0.3f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(delayDuration));
        }

        BowlSeq.OnComplete(() =>
        {
            downSequence = DOTween.Sequence();
            IsShakeEnd = true;
            downSequence.Append(transform.DOLocalRotate(new Vector3(80, 0, 0), 0.6f).SetRelative(true).SetEase(Ease.OutQuad));
            downSequence.Join(transform.DOLocalMoveZ(0.05f, 0.6f).SetRelative(true).SetEase(Ease.OutQuad));
            downSequence.Join(transform.DOLocalMoveY(-1f, 1f).SetRelative(true).SetEase(Ease.InQuad));

            downSequence.OnComplete(Init_bowl);
        });
    }

    public void Delay() => DelayTask().Forget();
    private async UniTask DelayTask()
    {
        gimmickSequence.isSequenceStop = true;
        await UniTask.WaitUntil(() => IsShakeEnd);
        gimmickSequence.isSequenceStop = false;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!IsShakeEnd && context.performed)
        {
            Init_bowl();
        }
    }
}
