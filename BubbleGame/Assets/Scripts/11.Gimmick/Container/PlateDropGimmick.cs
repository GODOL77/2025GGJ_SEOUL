using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlateDropGimmick : MonoBehaviour
{
    //파편 에셋 
    public GameObject fragments;
    private Vector3 initPos;

    //파편 튀기는 힘 배율 
    public float shatterForceRatio = 10f;
    //위로 얼마나 튀길지
    public float shatterYForce = 3f;

    //깨지는 소리
    //public AudioSource audioPlayer;
    //public AudioClip audioClip; //소리를 미리 저장 
    

    private bool isInteractAble;
    void Start()
    {
        isInteractAble = false;
        initPos = transform.position;
    }
    void Update()
    {
        if(isInteractAble)
        {

            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit hit))
                {
                    if(hit.collider.gameObject.name == gameObject.name)
                    {
                        ReturnToShelf();
                    }
                }
            }
        }
    }
    public void Play()
    {
        //예비 동작: 흔들흔들 하면서 앞으로 살살 기어간다. 
        //결국 선반에서 떨어지면서 자유낙하
        
    }
    private void OnCollisionExit(Collision other) 
    {
        if(other.gameObject.tag == "Shelf")
        {
            isInteractAble = true;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Background")
        {
            //부서진다.
            for(int i=0; i<3; i++)
            {
                //파편을 생성한다. 
                GameObject fragEach = Instantiate(fragments, transform.position, Quaternion.identity);
                //파편에 고유한 이름을 부여한다.
                fragEach.name = "fragment" + i;
                //파편이 날라갈 힘과 방향을 랜덤 값으로 결정한다. 
                int dirNum = Random.Range(0,1) == 1 ? -1 : 1;
                float forceNum = Random.Range(0,5);
                //파편을 튀긴다. 
                fragEach.GetComponent<Rigidbody>().AddForce(new Vector3(dirNum, shatterYForce, 0f) * shatterForceRatio);
            }
            //audioPlayer.PlayOneShot(audioClip);
            Destroy(gameObject);
        }    
    }
    private void ReturnToShelf()
    {
        //상호작용 비활성화 
        isInteractAble = false;
        //원래 위치로 복귀 
        transform.position = initPos;
    }

}
