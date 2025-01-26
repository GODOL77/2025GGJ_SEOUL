using System;
using Cysharp.Threading.Tasks;
using GamePlay;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Manager
{
    public partial class GameManager : Singleton<GameManager>
    {
        public AudioSource bgmSound;
        public AudioClip easyBGMClip;
        public AudioClip hardBGMClip;
        public float soundChangeDuration = 300f;

        public UnityEvent playerDieAction;

        public float playTime = 0f;
        public bool isGameEnd = false;

        public void Awake()
        {
            playerDieAction.AddListener(() => isGameEnd = true);
            playerDieAction.AddListener(SceneUtil.AddDieScene);
        }

        public void Start()
        {
            OnBGMSound().Forget();
        }

        public void Update()
        {
            if(isGameEnd) return;
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