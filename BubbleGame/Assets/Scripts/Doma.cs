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

    async UniTask StartPattern(float duration)
    {
        isPattern = true;
        /* 야채를 생성 애니메이션
            
        */
        await UniTask.Delay((int)(duration * 1000));
        if(isPattern) 
            Debug.Log("패턴 실행");
        else
            Debug.Log("패턴 해제");

    }



    private bool Check_Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == vegetable) 
                return true;
        }
        return false;
    }

    private bool Check_DragOut()
    {
        float dis = Vector3.Distance(Click_MousePos, InputManager.MousePosition);
        return dis > 500f;
    }

    async UniTask drag()
    {
        //Debug.Log("드래그");
        while (isClick)
        {
            Vector3 screenPosition = InputManager.MousePosition; // 마우스 화면 좌표
            screenPosition.z = Camera.main.WorldToScreenPoint(vegetable.transform.position).z; // 야채의 월드 Z 값
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            //vegetable.transform.position = worldPosition;
            vegetable.transform.position = Vector3.Lerp(
            vegetable.transform.position, worldPosition,  0.3f);
            await UniTask.Yield();
        }

        //Debug.Log("드래그 종료");
        if (Check_DragOut())
        {
            Vector3 screenPosition = InputManager.MousePosition; // 마우스 화면 좌표
            screenPosition.z = Camera.main.WorldToScreenPoint(vegetable.transform.position).z; // 야채의 월드 Z 값
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            
            /*
             * 야채 버리는 애니메이션
            */
            vegetable.transform.DOMove(worldPosition, 1f);
            vegetable.transform.DORotate(new Vector3(0, 0 , 180f), 1f, RotateMode.LocalAxisAdd);
            isPattern = false;
        }
        else
            vegetable.transform.position = BaseVegetablePos;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Check_Click() && isClick == false)
            {
                Debug.Log("야채 클릭");
                isClick = true;
                Click_MousePos = InputManager.MousePosition;
                drag().Forget();
            }
        }
        if (context.canceled)
        {
            isClick = false;
        }
    }
}
