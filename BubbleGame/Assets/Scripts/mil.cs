using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class mil : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mils;
    Sequence MilSeq;

    private void Start()
    {
        //init_bowl();
        shake_mil();
    }
    public void init_bowl()
    {
        MilSeq.Kill();
    }

    public void shake_mil()
    {
        MilSeq.Append(mils.transform.DOMove(new Vector3(0, 0, -0.3f), 2f).SetRelative(true).SetEase(Ease.OutQuad));
        MilSeq.Join(mils.transform.DORotate(new Vector3(30, 0, 0f), 2f).SetRelative(true).SetEase(Ease.OutQuad));
        MilSeq.Append(mils.transform.DORotate(new Vector3(30, 0, 0f), 3f).SetRelative(true).SetEase(Ease.OutQuad)).SetDelay(2f);
    }

}
