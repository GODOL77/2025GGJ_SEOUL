﻿using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gimmick
{
    public class GimmickSequence : MonoBehaviour
    {
        public bool isOnAwake = false;
        public bool isLoop = false;
        [HideInInspector] public bool isSequenceStop = false;
        public GimmickAction[] gimmicks;
        
        [SerializeField] private GimmickAction _currentGimmick;
        private IEnumerator _currentGimmickEnumerator;

        private Animator animator;

        public void Awake()
        {
            isSequenceStop = !isOnAwake;
            animator = GetComponent<Animator>();

            Init();
        }

        public void Update()
        {
            if(isSequenceStop) return;
            
            _currentGimmick.rateTimer.Current -= Time.deltaTime;
            if (_currentGimmick.rateTimer.IsMin)
            {
                _currentGimmick.action.Invoke();
                _currentGimmick.delay.Invoke();
                _currentGimmick.rateTimer.SetMax();
                _currentGimmick.loopCount.Current++;
                if (!_currentGimmick.isLoop && _currentGimmick.loopCount.IsMax)
                {
                    if (!_currentGimmickEnumerator.MoveNext())
                    {
                        if(isLoop) Init();
                        else enabled = false;
                    }
                    else
                        _currentGimmick = _currentGimmickEnumerator.Current as GimmickAction;
                }
            }
        }

        public void Init()
        {
            _currentGimmickEnumerator = gimmicks.GetEnumerator();
            if(_currentGimmickEnumerator.MoveNext())
                _currentGimmick = _currentGimmickEnumerator.Current as GimmickAction;
            else
                isSequenceStop = true;
            foreach (var gimmick in gimmicks)
                gimmick.rateTimer.SetMax();

            enabled = true;
        }
        public void Play() => isSequenceStop = false;
        public void Stop() => isSequenceStop = true;
        
        /// <summary>
        /// 적절한 애니메이션이 아니면 Sequence 중단
        /// </summary>
        public void IsCurrentAnimationClipOrStop(string clipName)
        {
            CheckAnimationClip(clipName).Forget();
        }

        private async UniTask CheckAnimationClip(string clipName)
        {
            isSequenceStop = true;
            
            await UniTask.WaitForFixedUpdate();
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            await UniTask.WaitUntil(() => stateInfo.IsName(clipName));

            isSequenceStop = false;
        }
    }
}