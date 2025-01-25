using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Linq.Expressions;
using GamePlay;
using System.Runtime.InteropServices;


public class Doma : MonoBehaviour, IInteract
{
    private bool isClick = false;
    private bool isPattern = false;


    public ParticleSystem VegetableSlayer;


    public GameObject vegetable;
    public Vector3 BaseVegetablePos;
    private Vector3 Click_MousePos;

    public bool IsComplete = false;
    private void Awake()
    {
        BaseVegetablePos = vegetable.transform.position;
        PlayPattern(5f).Forget();
    }

    async UniTask PlayPattern(float LimiteTime)
    {
        transform.localPosition = BaseVegetablePos;
        transform.localRotation = Quaternion.Euler(0,0,-90);
        IsComplete = false;
        await UniTask.Delay((int)(1000 * LimiteTime));
        if (IsComplete)
        {
            Debug.Log("파훼");
        }
        else
        {
            vegetable.transform.DOLocalMoveZ(0.7f, 0.6f).SetEase(Ease.InQuad);
            vegetable.transform.DOLocalMoveY(0.7f, 0.4f).SetEase(Ease.InQuad);
            vegetable.transform.DOLocalRotate(new Vector3(60f,0,0), 0.5f).SetEase(Ease.InQuad).SetDelay(0.2f);
            vegetable.transform.DOLocalMoveY(-0.5f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.3f).OnComplete(()=>
            {

                PlayParticleTwice().Forget();
            });
        }
    }
    private async UniTask PlayParticleTwice()
    {

        for (int i = 0; i < 2; i++) // 2번 반복
        {
            VegetableSlayer.Play(); // 파티클 시스템 재생
            await UniTask.Delay((int)((VegetableSlayer.main.duration + VegetableSlayer.main.startLifetime.constantMax) * 1000));
            // 파티클 재생 완료 후 대기
        }
    }




        private bool Check_DragOut()
    {
        float dis = Vector3.Distance(Click_MousePos, InputManager.MousePosition);
        return dis > 300f;
    }


    async UniTask drag()
    {
        while (isClick && Mouse.current.leftButton.isPressed)
        {
            Vector3 screenPosition = InputManager.MousePosition; // ���콺 ȭ�� ��ǥ
            screenPosition.z = Camera.main.WorldToScreenPoint(vegetable.transform.position).z; // ��ä�� ���� Z ��
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            //vegetable.transform.position = worldPosition;
            vegetable.transform.position = Vector3.Lerp(
            vegetable.transform.position, worldPosition,  0.3f);
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
        }
        else
            vegetable.transform.position = BaseVegetablePos;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("클릭");
            isClick = true;
            Click_MousePos = InputManager.MousePosition;
            drag().Forget();
        }

    }
}
