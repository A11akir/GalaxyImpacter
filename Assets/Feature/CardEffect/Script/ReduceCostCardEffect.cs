using System;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ReduceCostCardEffect : PassiveCardEffect
    {
        [SerializeReference] private IDynamicCostValueSource _valueSource;

        public override IDisposable BindPassiveEffect(CardAndHealthEntityOwnerData owner, GameSessionPlayerData side, CardStatsData card)
        {
            return _valueSource.Subscribe(owner, side, value =>
                card.Cost = Mathf.Max(0, card.BaseCost - value));
        }
    }
}
