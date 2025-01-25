using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Gimmick.Container
{
    public class Sink : MonoBehaviour, IInteract
    {
        // 수도꼭지 오브젝트
        public GameObject faucetObject;
        // 수도꼭지의 회전 횟수 n / 360
        public StatusValue<int> faucetRotateCount = new(0, 3);
        // 싱크대 안에 물이 얼마나 찼는지
        public StatusValue<float> pool = new(0, 0, 1);

        public GimmickSequence gimmickSequence;
        // 활성화 되었을때 아웃라인 추가하는 용도
        public GimmickMaterialControl gimmickMaterialControl;

        private bool isFaucetOn = false; // 수도꼭지가 틀어져 있는지
        private CancellationTokenSource _cancelToken = new();
        private Tween faucetRotateTween;

        // 기믹이 동작 조건을 만족해 동작중인지
        public bool IsGimmickActive => faucetRotateCount.IsMax;

        public void Init()
        {
            isFaucetOn = false;
            faucetRotateCount.SetMin();
            pool.SetMin();

            Stop();
        }
        
        public void Play()
        {
            if (faucetRotateCount.IsMin)
            {
                gimmickMaterialControl.AddMaterial();
            }

            faucetRotateCount.Current++;
            if (faucetRotateTween != null) faucetRotateTween.Pause();
            faucetRotateTween = faucetObject.transform.DORotate(new Vector3(0, 0, (360f / faucetRotateCount.Max) * faucetRotateCount.Current), 0.4f);
            
            if (faucetRotateCount.IsMax)
            {
                isFaucetOn = true;
                gimmickSequence.Stop();
                ActiveFaucet(_cancelToken.Token).Forget();
            }
        }

        public void Stop()
        {
            _cancelToken.Cancel();
            _cancelToken.Dispose();
            
            gimmickMaterialControl.RemoveMaterial();
        }

        private async UniTask ActiveFaucet(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForEndOfFrame();
                pool.Current += Time.deltaTime;
                
                if(pool.IsMax) Stop();
            }
        }

        public void GimmickDelay() => GimmickDelayTask().Forget();
        private async UniTask GimmickDelayTask()
        {
            gimmickSequence.isSequenceStop = true;
            await UniTask.WaitUntil(() => !faucetRotateCount.IsMax || pool.IsMax);
            gimmickSequence.isSequenceStop = false;
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (isFaucetOn) Init();
                else faucetRotateCount.Current--;
            }
        }
    }
}