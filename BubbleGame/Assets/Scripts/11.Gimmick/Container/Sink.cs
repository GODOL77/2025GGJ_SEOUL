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

        // 수도꼭지가 몇초동안 회전양을 다 회전할지
        public StatusValue<float> faucetRotateTime = new(0, 0, 3);
        // 수도 꼭지 회전양
        public StatusValue<float> faucetRotateAmount = new(0, 0, 720);
        // 싱크대 안에 물이 얼마나 찼는지
        public StatusValue<float> pool = new(0, 0, 1);
        // 수도꼭치 드래그 힘
        public float dragPower = 10f;

        public GimmickSequence gimmickSequence;
        // 활성화 되었을때 아웃라인 추가하는 용도
        public GimmickMaterialControl gimmickMaterialControl;

        private bool isFaucetOn = false; // 수도꼭지가 틀어져 있는지
        private bool isDrag = false;
        private CancellationTokenSource _cancelToken = new();
        private CancellationTokenSource _faucetRotateCancelToken = new();
        private CancellationTokenSource _dragCancelToken = new();

        public void Init()
        {
            isFaucetOn = false;
            pool.SetMin();

            Stop();
        }
        
        public void Play()
        {
            if(!gimmickMaterialControl.HasMaterial)
                gimmickMaterialControl.AddMaterial();
            RotateTask(_faucetRotateCancelToken.Token).Forget();
        }

        public void Stop()
        {
            _cancelToken.Cancel();
            _cancelToken.Dispose();
            _cancelToken = new();
            
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

        private async UniTask RotateTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForFixedUpdate();
                faucetRotateTime.Current += Time.fixedDeltaTime * Time.timeScale;
                faucetRotateAmount.Current = faucetRotateAmount.Max / faucetRotateTime.Max * faucetRotateTime.Current;
                var angle = faucetObject.transform.localEulerAngles;
                angle.z = faucetRotateAmount.Current;
                faucetObject.transform.localEulerAngles = angle;
                
                if (faucetRotateAmount.IsMax)
                {
                    isFaucetOn = true;
                    ActiveFaucet(_cancelToken.Token).Forget();
                }
            }
        }
        
        public void GimmickDelay() => GimmickDelayTask().Forget();
        private async UniTask GimmickDelayTask()
        {
            gimmickSequence.isSequenceStop = true;
            await UniTask.WaitUntil(() => (faucetRotateTime.IsMax  && pool.IsMax) || faucetRotateTime.IsMin);
            gimmickSequence.isSequenceStop = false;
        }

        private async UniTask DragTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForEndOfFrame();
                Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition);
                RaycastHit hit;

                if (isDrag && Physics.Raycast(ray, out hit, float.MaxValue) &&
                    hit.transform.gameObject == gameObject)
                {
                    if (InputManager.MouseDrag.x < 0f)
                    {
                        faucetRotateTime.Current += InputManager.MouseDrag.x * dragPower * Time.fixedDeltaTime * Time.timeScale;
                        faucetRotateAmount.Current = faucetRotateAmount.Max / faucetRotateTime.Max * faucetRotateTime.Current;
                        var angle = faucetObject.transform.localEulerAngles;
                        angle.z = faucetRotateAmount.Current;
                        faucetObject.transform.localEulerAngles = angle;
                    }
                }
                else
                    break;
            }
            
            if(!faucetRotateAmount.IsMin) RotateTask(_faucetRotateCancelToken.Token).Forget();
            else
            {
                gimmickMaterialControl.RemoveMaterial();
            }
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _faucetRotateCancelToken.Cancel();
                _faucetRotateCancelToken.Dispose();
                _faucetRotateCancelToken = new();
                DragTask(_dragCancelToken.Token).Forget();
                isDrag = true;
            }
            else if (context.canceled)
            {
                isDrag = false;
                _dragCancelToken.Cancel();
                _dragCancelToken.Dispose();
                _dragCancelToken = new();
            }
        }
    }
}