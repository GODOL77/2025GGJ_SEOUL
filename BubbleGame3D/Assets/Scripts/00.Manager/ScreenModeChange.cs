using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ScreenModeChange : MonoBehaviour
{
    private bool isFullScreenMode;

    void Start()
    {
        isFullScreenMode = false;
    }
    public void SwitchScreenMode()
    {
        if (isFullScreenMode)
        {
            SetWindowMode();
        }
        else
        {
            SetFullScreenMode();
        }
    }

    void SetFullScreenMode()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        isFullScreenMode = true;
    }
    void SetWindowMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        isFullScreenMode = false;
    }
}
