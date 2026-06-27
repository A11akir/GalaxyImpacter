using System;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ReduceCostCardEffect : PassiveCardEffect
    {
        [SerializeReference] private IDynamicCostValueSource _valueSource;

        public override IDisposable Activate(CardAndHealthEntityOwnerData owner, CardStatsData card, Action onChanged)
        {
            return _valueSource.Subscribe(owner, value =>
            {
                card.Cost = Mathf.Max(0, card.BaseCost - value);
                onChanged?.Invoke(); 
            });
        }
    }
}