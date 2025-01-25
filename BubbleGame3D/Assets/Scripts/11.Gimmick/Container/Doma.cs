using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Linq.Expressions;
using GamePlay;


public class Doma : MonoBehaviour, IInteract
{
    private bool isClick = false;
    private bool isPattern = false;

    public GameObject vegetable;
    public Vector3 BaseVegetablePos;
    private Vector3 Click_MousePos;

    private void Awake()
    {
        BaseVegetablePos = vegetable.transform.position;
        //StartPattern(5f).Forget();
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

            if(InputManager.MousePosition.x < BaseVegetablePos.x)
                vegetable.transform.DOLocalMoveX(0.8f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.4f);
            else
                vegetable.transform.DOLocalMoveX(-0.8f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.4f);

            vegetable.transform.DOLocalMoveZ(-0.5f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.4f);
            vegetable.transform.DOLocalMoveY(-0.5f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.4f);
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

        if (context.canceled)
        {
            Debug.Log("클릭 취소");
            isClick = false;
        }
    }
}
