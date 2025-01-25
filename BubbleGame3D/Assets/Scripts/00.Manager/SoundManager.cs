using UnityEngine.Audio;
using Util;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioMixer mixer;
        
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SetVolume(string audioName, float value) => mixer.SetFloat(audioName, value);
        public float GetVolume(string audioName) => mixer.GetFloat(audioName, out float value) ? value : 0;
    }
}