using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using GamePlay;
using Gimmick;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class PlateDropGimmick : MonoBehaviour, IInteract
{
    //파편 에셋 
    public GameObject fragments;

    // 몇번 움직이고 난뒤에 드랍할 것인지
    public StatusValue<int> dropCount = new(0,0,3);
    //파편 튀기는 힘 배율 
    public float shatterForceRatio = 10f;
    //위로 얼마나 튀길지
    public float shatterYForce = 3f;

    public GimmickMaterialControl gimmickMaterialControl;

    //깨지는 소리
    //public AudioSource audioPlayer;
    //public AudioClip audioClip; //소리를 미리 저장 

    private Rigidbody _rigidbody;
    private Vector3 initPos;
    private Vector3 originAngle;
    private bool isInteractAble;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        isInteractAble = false;
        initPos = transform.localPosition;
        originAngle = transform.localEulerAngles;
    }
    
    private void OnCollisionEnter(Collision other)
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
        Init();
    }
    
    public void Play()
    {
        //예비 동작: 흔들흔들 하면서 앞으로 살살 기어간다. 
        //결국 선반에서 떨어지면서 자유낙하

        dropCount.Current++;
        if (dropCount.IsMax && !isInteractAble)
        {
            if(!gimmickMaterialControl.HasMaterial) gimmickMaterialControl.AddMaterial();
            
            isInteractAble = true;
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _rigidbody.AddTorque(Vector3.one);
        }
    }

    public void Init()
    {
        gimmickMaterialControl.RemoveMaterial();
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        ReturnToShelf();
    }
    
    private void ReturnToShelf()
    {
        //상호작용 비활성화 
        isInteractAble = false;
        //원래 위치로 복귀 
        transform.localPosition = initPos;
        transform.localEulerAngles = originAngle;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(isInteractAble && context.performed)
        {
            Init();
        }
    }
}
