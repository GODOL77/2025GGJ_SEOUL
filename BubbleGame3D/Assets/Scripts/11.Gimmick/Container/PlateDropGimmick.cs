using Cysharp.Threading.Tasks;
using GamePlay;
using Gimmick;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlateDropGimmick : MonoBehaviour, IInteract
{
    //파편 에셋 
    public GameObject fragments;

    //파편 튀기는 힘 배율 
    public float shatterForceRatio = 10f;
    //위로 얼마나 튀길지
    public float shatterYForce = 3f;

    public GimmickSequence gimmickSequence;
    public GimmickMaterialControl gimmickMaterialControl;

    //깨지는 소리
    //public AudioSource audioPlayer;
    //public AudioClip audioClip; //소리를 미리 저장 

    private Collider collider;
    private bool isInteractAble;

    public void Awake()
    {
        collider = GetComponentInChildren<Collider>();
    }

    void Start()
    {
        Init();
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
            var dirNum = Mathf.Pow(-1, i);
            //파편을 튀긴다. 
            var force = new Vector3(dirNum, shatterYForce, 0f);
            fragEach.GetComponent<Rigidbody>().AddForce(force.normalized * shatterForceRatio);
        }
        //audioPlayer.PlayOneShot(audioClip);
        Init();
    }
    
    public void Play()
    {
        if(!gimmickMaterialControl.HasMaterial) gimmickMaterialControl.AddMaterial();

        collider.isTrigger = false;
        isInteractAble = true;
    }

    public void Init()
    {
        gimmickMaterialControl.RemoveMaterial();
        //상호작용 비활성화 
        isInteractAble = false;
        collider.isTrigger = true;
    }

    public void Delay() => DelayTask().Forget();

    private async UniTask DelayTask()
    {
        gimmickSequence.isSequenceStop = true;
        await UniTask.WaitUntil(() => !collider.isTrigger);
        gimmickSequence.isSequenceStop = false;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(isInteractAble && context.performed)
        {
            Init();
        }
    }
}
