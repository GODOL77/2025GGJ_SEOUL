using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;

namespace Gimmick.Container
{
    public class Sink : MonoBehaviour
    {
        // 싱크대 안에 물이 얼마나 찼는지
        public StatusValue<float> pool = new(0, 0, 1);


        private CancellationTokenSource _cancelToken;
        
        public void Play()
        {
            ActiveFaucet(_cancelToken.Token).Forget();
        }

        public void Stop()
        {
            if (_cancelToken != null)
            {
                _cancelToken.Cancel();
                _cancelToken.Dispose();
                _cancelToken = null;
            }
        }

        public void ReSet()
        {
            pool.SetMin();
        }

        private async UniTask ActiveFaucet(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForEndOfFrame();
                pool.Current += Time.deltaTime;
            }
        }
    }
}