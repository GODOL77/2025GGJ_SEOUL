using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleBouncer : MonoBehaviour
{
    //버블이 바닥에 닿았는지 여부 
    private bool isGround;

    //스페이스바가 활성화되었는지 
    private bool isSpaceActive;

    //버블이 바닥에 닿은 직후 스페이스바 입력 활성화 시간 길이 
    //50은 50밀리세컨드를 의미 
    public float spaceActiveTimeLength = 50;

    //마지막으로 스페이스바 입력 활성화한 시간 
    private float lastActiveTime;

    // Start is called before the first frame update
    void Start()
    {
        isGround = false;
        isSpaceActive = false;
        lastActiveTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGround)
        {
            //잠깐 스페이스바 입력 활성화하기 
            StartCoroutine(ActiveSpace());
            //한 번 실행하고 isGround = false로. 
            isGround = false;
        }
        if(isSpaceActive && Input.GetKeyDown(KeyCode.Space))
        {
            //힘주기기
        }
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Background")
        {
            isGround = true;
        }    
    }

    //확실하게 isGround false로 만들기기
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Background")
        {
            isGround = false;
        }
    }
    IEnumerator ActiveSpace()
    {
        isSpaceActive = true;
        yield return new WaitForSeconds(spaceActiveTimeLength);
        isSpaceActive = false;
    }
}
