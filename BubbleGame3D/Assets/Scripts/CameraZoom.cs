using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CameraZoom : MonoBehaviour
{
    private void Start()
    {
        //SetResolution(); // �ʱ⿡ ���� �ػ� ����
    }

    public void SetResolution(int x, int y)
    {
        int setWidth = x; // ����� ���� �ʺ�
        int setHeight = y; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
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