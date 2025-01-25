using System;
using UnityEngine.Events;
using Util;

namespace Gimmick
{
    [System.Serializable]
    public class GimmickAction
    {
        public bool isLoop = false;
        public StatusValue<int> loopCount = new(0, 0, 1);
        public StatusValue<float> rateTimer = new();
        public UnityEvent action;
        public UnityEvent delay;
        public Func<bool> onDelayTask;
    }
}