using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using GamePlay;
using System.Threading;
using Gimmick;


public class Doma : MonoBehaviour, IInteract
{
    private bool isClick = false;
    private bool isPattern = false;

    public GimmickSequence gimmickSequence;
    public GimmickMaterialControl gimmickMaterialControl;
    public ParticleSystem VegetableSlayer;
    public Collider attackCollider;

    public GameObject vegetable;
    private Vector3 BaseVegetablePos;
    private Vector3 Click_MousePos;
    private CancellationTokenSource _dragCancelToken = new();

    public bool IsComplete = false;
    private void Awake()
    {
        BaseVegetablePos = vegetable.transform.localPosition;
        gameObject.SetActive(false);
        attackCollider.gameObject.SetActive(false);
    }

    public void Init()
    {
        transform.localPosition = BaseVegetablePos;
        transform.localRotation = Quaternion.Euler(0, 0, -90);
    }

    public void OnDoma()
    {
        if (!gimmickMaterialControl.HasMaterial) gimmickMaterialControl.AddMaterial();
        Init();
    }

    public void PlayKnife()
    {
        IsComplete = false;

        vegetable.transform.DOLocalMoveZ(0.7f, 0.6f).SetEase(Ease.InQuad);
        vegetable.transform.DOLocalMoveY(0.7f, 0.4f).SetEase(Ease.InQuad);
        vegetable.transform.DOLocalRotate(new Vector3(60f, 0, 0), 0.5f).SetEase(Ease.InQuad).SetDelay(0.2f);
        vegetable.transform.DOLocalMoveY(-0.5f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.3f).OnComplete(() =>
        {
            if(!IsComplete) PlayParticleTwice().Forget();
        });
    }

    private async UniTask PlayParticleTwice()
    {
        gimmickSequence.isSequenceStop = true;
        VegetableSlayer.Play(); // 파티클 시스템 재생
        attackCollider.gameObject.SetActive(true);
        await UniTask.Delay((int)((VegetableSlayer.main.duration + VegetableSlayer.main.startLifetime.constantMax) * 1000));
        attackCollider.gameObject.SetActive(false);
        gimmickSequence.isSequenceStop = false;
    }

    private bool Check_DragOut()
    {
        float dis = Vector3.Distance(Click_MousePos, InputManager.MousePosition);
        return dis > 300f;
    }

    async UniTask drag(CancellationToken token)
    {
        while (!token.IsCancellationRequested && Mouse.current.leftButton.isPressed)
        {
            Vector3 screenPosition = InputManager.MousePosition; // ���콺 ȭ�� ��ǥ
            screenPosition.z = Camera.main.WorldToScreenPoint(vegetable.transform.position).z; // ��ä�� ���� Z ��
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            //vegetable.transform.position = worldPosition;
            vegetable.transform.position = Vector3.Lerp(vegetable.transform.position, worldPosition,  0.3f);
            await UniTask.Yield();
        }

        if (Check_DragOut())
        {
            Vector3 screenPosition = InputManager.MousePosition; // ���콺 ȭ�� ��ǥ
            screenPosition.z = Camera.main.WorldToScreenPoint(vegetable.transform.position).z; // ��ä�� ���� Z ��
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            
            vegetable.transform.DOLocalMove(worldPosition, 0.5f);
            vegetable.transform.DOLocalRotate(new Vector3(540f, 0 , 0), 2f, RotateMode.LocalAxisAdd);

            if(vegetable.transform.position.x < worldPosition.x)
                vegetable.transform.DOLocalMoveX(0.8f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.4f);
            else
                vegetable.transform.DOLocalMoveX(-0.8f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.4f);

            vegetable.transform.DOLocalMoveZ(0.5f, 0.3f).SetEase(Ease.InQuad).SetDelay(0.4f);
            vegetable.transform.DOLocalMoveY(-0.5f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.4f);
            IsComplete = true;
            isPattern = false;
            
            gimmickSequence.Init();
        }
        else
            vegetable.transform.position = BaseVegetablePos;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isClick = true;
            Click_MousePos = InputManager.MousePosition;
            _dragCancelToken.Cancel();
            _dragCancelToken.Dispose();
            _dragCancelToken = new();
            drag(_dragCancelToken.Token).Forget();
        }
        else if (context.canceled)
        {
            _dragCancelToken.Cancel();
            _dragCancelToken.Dispose();
            _dragCancelToken = new();
        }
    }
}
