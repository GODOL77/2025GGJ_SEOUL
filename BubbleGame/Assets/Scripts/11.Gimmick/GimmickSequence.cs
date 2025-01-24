using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gimmick
{
    public class GimmickSequence : MonoBehaviour
    {
        public GimmickAction[] gimmicks;

        private Animator animator;
        
        [SerializeField] private GimmickAction _currentGimmick;
        private IEnumerator _currentGimmickEnumerator;

        private bool isSequenceStop = false;

        public void Awake()
        {
            _currentGimmickEnumerator = gimmicks.GetEnumerator();
            if(_currentGimmickEnumerator.MoveNext())
                _currentGimmick = _currentGimmickEnumerator.Current as GimmickAction;
            else
                isSequenceStop = true;

            animator = GetComponent<Animator>();
            
            foreach (var gimmick in gimmicks)
            {
                gimmick.rateTimer.SetMax();
            }
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
                        enabled = false;
                    else
                        _currentGimmick = _currentGimmickEnumerator.Current as GimmickAction;
                }
            }
        }
        
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