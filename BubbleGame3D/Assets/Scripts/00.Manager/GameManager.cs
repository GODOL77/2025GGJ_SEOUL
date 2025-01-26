using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GamePlay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Util;

namespace Manager
{
    public partial class GameManager : Singleton<GameManager>
    {
        public Volume levelChangeVolume;
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

        public async UniTask OnLevelUP()
        {
            float increaseMultiple = 1;
            while (true)
            {
                await UniTask.Yield();
                levelChangeVolume.weight += increaseMultiple * Time.deltaTime;

                if (levelChangeVolume.weight >= 1f) increaseMultiple = -1;
                else if(levelChangeVolume.weight <= 0) increaseMultiple = 1;
            }
        }

        public async UniTask OnBGMSound()
        {
            bgmSound.clip = easyBGMClip;
            bgmSound.Play();
            await UniTask.WaitForSeconds(soundChangeDuration);
            bgmSound.clip = hardBGMClip;
            bgmSound.Play();
            OnLevelUP().Forget();
        }

        public void InvokePlayerDieAction(float time = 0f)
        {
            StopAllCoroutines();
            StartCoroutine(InvokePlayerDieActionEnumerator(time));
        }

        public IEnumerator InvokePlayerDieActionEnumerator(float time)
        {
            yield return new WaitForSeconds(time);
            playerDieAction?.Invoke();
        }
    }
}