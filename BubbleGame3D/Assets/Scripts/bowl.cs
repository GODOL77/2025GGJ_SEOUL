using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class bowl : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bowls;
    Sequence BowlSeq;

    public Vector3 Base_Pos;

    private void Start()
    {
        init_bowl();
        shake_bowl();
    }
    public void init_bowl()
    {
        BowlSeq.Kill();
        Debug.Log(Base_Pos);
        gameObject.transform.DOMove(Base_Pos, 0.1f);
       // gameObject.transform.position = Base_Pos;
        //gameObject.transform.rotation = Quaternion.EulerRotation(new Vector3(0,0,-4));
    }

    public void shake_bowl()
    {

        BowlSeq.Append(bowls.transform.DORotate(new Vector3(0, 0, 8), 1f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad));
        BowlSeq.Join(bowls.transform.DOMoveZ(-0.01f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad));

        BowlSeq.Append(bowls.transform.DORotate(new Vector3(0, 0, 8), 0.8f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(2f));
        BowlSeq.Join(bowls.transform.DOMoveZ(-0.03f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(2f));

        BowlSeq.Append(bowls.transform.DORotate(new Vector3(0, 0, 8), 0.6f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(3.6f));
        BowlSeq.Join(bowls.transform.DOMoveZ(-0.03f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(4f));

        BowlSeq.Append(bowls.transform.DORotate(new Vector3(0, 0, 8), 0.4f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(4.8f));
        BowlSeq.Join(bowls.transform.DOMoveZ(-0.04f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(4.8f));

        BowlSeq.Append(bowls.transform.DORotate(new Vector3(0, 0, 8), 0.3f).SetRelative(true).SetLoops(6, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(5.6f));
        BowlSeq.Join(bowls.transform.DOMoveZ(-0.04f, 1f).SetRelative(true).SetLoops(2, LoopType.Incremental).SetEase(Ease.OutQuad).SetDelay(5.6f));

        BowlSeq.Append(bowls.transform.DORotate(new Vector3(80, 0, 0), 0.6f).SetRelative(true).SetEase(Ease.OutQuad).SetDelay(7.4f));
        BowlSeq.Join(bowls.transform.DOMoveY(-1f, 1f).SetRelative(true).SetEase(Ease.InQuad).SetDelay(7.6f));

    }
}
