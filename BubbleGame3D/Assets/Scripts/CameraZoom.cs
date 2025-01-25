using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CameraZoom : MonoBehaviour
{
    private void Start()
    {
        //SetResolution(); // 초기에 게임 해상도 고정
    }

    public void SetResolution(int x, int y)
    {
        int setWidth = x; // 사용자 설정 너비
        int setHeight = y; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    public void pre()
    {
        Debug.Log("aas");
        zoomin().Forget();
    }

    public async UniTask zoomin()
    {
        for (int i = 0; i < 30; i++)
        {
            SetResolution(1920 - (int)((float)(i)/30 * 960), 1080);
            await UniTask.Delay((int)(0.1 * 1000));
        }
        Debug.Log("end");
    }
}