using UnityEngine;
using UnityEngine.Audio;
using Util;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioMixer mixer;
        
        public void Awake()
        {
            mixer = Resources.Load<AudioMixer>("Audio/Default Mixer");
            DontDestroyOnLoad(gameObject);
        }

        public static void SetVolume(string audioName, float value) => Instance.mixer.SetFloat(audioName, value);
        public static float GetVolume(string audioName) => Instance.mixer.GetFloat(audioName, out float value) ? value : 0;
    }
}