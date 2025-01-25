using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CutSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> CutImages = new List<GameObject>();
    public List<GameObject> EmotionImages = new List<GameObject>();
    Sequence ShowSequence;

    public void ScenceLoad()
    {
        //씬로드 코드
        Debug.Log("로드");
    }
}
