// ReduceCostCardEffect.cs
using System;
using Feature.GameSessionData;
using UnityEngine;
using R3;

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
                Debug.Log($"[ReduceCostCardEffect] card={card.Name}, new Cost={card.Cost}, hash={card.GetHashCode()}");
                onChanged();
            });
        }
    }
}