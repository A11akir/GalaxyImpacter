using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Feature.AI
{
    public class AIRandomSelectSystem
    {
        public Selector<T> RandomSelectValue<T>(List<T> list)
        {
            return new Selector<T>(list);
        }
        
        public class Selector<T>
        {
            private List<T> _list;
            private T _selectedValue;
            private Action<T> _onComplete;
            
            public Selector(List<T> list)
            {
                _list = list;
                
                if (_list == null || _list.Count == 0)
                {
                    _selectedValue = default(T);
                }
                else
                {
                    int randomIndex = UnityEngine.Random.Range(0, _list.Count);
                    _selectedValue = _list[randomIndex];
                }
            }
            
            public void AIImitation()
            {
                float delay = UnityEngine.Random.Range(0.5f, 2f);
                DOVirtual.DelayedCall(delay, () =>
                {
                    _onComplete?.Invoke(_selectedValue);
                });
            }
            
            public Selector<T> OnComplete(Action<T> onComplete)
            {
                _onComplete = onComplete;
                return this;
            }
        }
    }
}