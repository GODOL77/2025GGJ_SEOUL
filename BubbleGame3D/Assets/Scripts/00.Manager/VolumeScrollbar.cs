using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScrollbar : MonoBehaviour
{
    public Scrollbar scrollbar;

    public void MasterVol()
    {   
        float logValue = Mathf.Lerp(-80f, 0f, scrollbar.value);
        SoundManager.Instance.SetVolume("Master", logValue);
    }
    public void BGMVol()
    {   
        float logValue = Mathf.Lerp(-80f, 0f, scrollbar.value);
        SoundManager.Instance.SetVolume("BGM", logValue);
    }
    public void EffectVol()
    {   
        float logValue = Mathf.Lerp(-80f, 0f, scrollbar.value);
        SoundManager.Instance.SetVolume("Effect", logValue);
    }
}
