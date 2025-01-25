using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public AudioSource bgmSound;
        public AudioClip easyBGMClip;
        public AudioClip hardBGMClip;
        public float soundChangeDuration = 300f;

        public float playTime = 0f;

        public void Start()
        {
            OnBGMSound().Forget();
        }

        public void Update()
        {
            playTime += Time.deltaTime;
        }

        public async UniTask OnBGMSound()
        {
            bgmSound.clip = easyBGMClip;
            bgmSound.Play();
            await UniTask.WaitForSeconds(soundChangeDuration);
            bgmSound.clip = hardBGMClip;
            bgmSound.Play();
        }
    }
}