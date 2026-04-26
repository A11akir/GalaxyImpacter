using System;
using System.Collections.Generic;
using DG.Tweening;

namespace Feature.AI
{
    public class AIActionExecutor
    {

        public void ExecuteDelayed(Action action, float? customDelay = null)
        {
            float delay = customDelay ?? UnityEngine.Random.Range(0.5f, 2f);
            DOVirtual.DelayedCall(delay, () => action?.Invoke());
        }
        
        public void SelectAndExecute<T>(List<T> options, Action<T> onSelected, float? customDelay = null)
        {
            if (options == null || options.Count == 0)
            {
                onSelected?.Invoke(default(T));
                return;
            }

            T selected = options[UnityEngine.Random.Range(0, options.Count)];
            ExecuteDelayed(() => onSelected?.Invoke(selected), customDelay);
        }
    }
}