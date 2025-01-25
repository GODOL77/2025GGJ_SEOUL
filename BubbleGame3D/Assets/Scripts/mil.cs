using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class mil : MonoBehaviour
{
    // Start is called before the first frame update

    Sequence MilSeq;

    public Vector3 Base_Pos;
    private void Start()
    {
        init_mil();
        shake_mil();
    }
    public void init_mil()
    {
        MilSeq.Kill();
        transform.localPosition = Base_Pos;
        transform.localRotation = Quaternion.identity;
    }

    public void shake_mil()
    {
        MilSeq.Append(transform.DOLocalMove(new Vector3(0, -0.025f, 0.08f), 3f).SetRelative(true).SetEase(Ease.OutQuad));
        MilSeq.Join(transform.DOLocalRotate(new Vector3(40, 0, 0f), 4f).SetRelative(true).SetEase(Ease.OutQuad));
        
        MilSeq.Append(transform.DOLocalMove(new Vector3(0, -1.2f, 0.5f), 5f).SetRelative(true).SetEase(Ease.InSine).SetDelay(3.4f));
        MilSeq.Join(transform.DOLocalRotate(new Vector3(145, 0, 0f), 4f).SetRelative(true).SetEase(Ease.InSine).SetDelay(3.3f));


    }

}
