using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Gimmick;
public class bowl : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bowls;

    public Vector3 Base_Pos;

    public GimmickSequence gimmickSequence;

    Sequence BowlSeq;
    private bool isShakeEnd;
    public bool IsShakeEnd
    {
        get => isShakeEnd;
        private set => isShakeEnd = value;
    }

    public void Init_bowl()
    {
        BowlSeq.Kill();
        IsShakeEnd = false;
        Debug.Log(Base_Pos);
        gameObject.transform.localPosition = Base_Pos; 
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, -4);
    }

    public void Shake_bowl()
    {
        BowlSeq = DOTween.Sequence();
        BowlSeq.Append(bowls.transform.DORotate(new Vector3(0, 0, 8), 1f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad));
        BowlSeq.Join(bowls.transform.DOMoveZ(-0.01f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad));

        BowlSeq.Append(transform.DOLocalRotate(new Vector3(0, 0, 8), 0.8f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(2f));
        BowlSeq.Join(transform.DOLocalMoveZ(0.01f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(2f));

        BowlSeq.Append(transform.DOLocalRotate(new Vector3(0, 0, 8), 0.6f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(3.6f));
        BowlSeq.Join(transform.DOLocalMoveZ(0.01f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(4f));

        BowlSeq.Append(transform.DOLocalRotate(new Vector3(0, 0, 8), 0.4f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(4.8f));
        BowlSeq.Join(transform.DOLocalMoveZ(0.01f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(4.8f));

        BowlSeq.Append(transform.DOLocalRotate(new Vector3(0, 0, 8), 0.3f).SetRelative(true).SetLoops(6, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(5.6f));
        BowlSeq.Join(transform.DOLocalMoveZ(0.01f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(5.6f));

        BowlSeq.Append(transform.DOLocalRotate(new Vector3(80, 0, 0), 0.6f).SetRelative(true).SetEase(Ease.OutQuad).SetDelay(7.4f));
        BowlSeq.Join(transform.DOLocalMoveZ(0.05f, 0.6f).SetRelative(true).SetEase(Ease.OutQuad).SetDelay(7.4f));
        BowlSeq.Join(transform.DOLocalMoveY(-1f, 1f).SetRelative(true).SetEase(Ease.InQuad).SetDelay(7.6f));

        BowlSeq.OnComplete(() => IsShakeEnd = true);
    }

    public void Delay() => DelayTask().Forget();
    private async UniTask DelayTask()
    {
        gimmickSequence.isSequenceStop = true;
        await UniTask.WaitUntil(() => IsShakeEnd);
        gimmickSequence.isSequenceStop = false;
    }
}
