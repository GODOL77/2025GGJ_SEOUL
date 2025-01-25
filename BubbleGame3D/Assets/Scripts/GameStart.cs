using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        StartCameraMove();
    }
    public void StartCameraMove()
    {
        //Time.timeScale = 0.0f;
        gameObject.transform.DOLocalMoveZ(0.5f, 0f);
        gameObject.transform.DOLocalMoveZ(1f, 2f).SetEase(Ease.InQuart).SetRelative(true);
        
    }

}
