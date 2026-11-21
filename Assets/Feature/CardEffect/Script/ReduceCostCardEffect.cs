using System;
using Feature.Card.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ReduceCostCardEffect : PassiveCardEffect
    {
        [SerializeReference] private IDynamicCostValueSource _valueSource;

        public override IDisposable BindPassiveEffect(
            CardAndHealthEntityOwnerData owner,
            GameSessionPlayerData side,
            CardStatsData card,
            CalculateCardCostSystem costSystem,
            CardCastService castService)
        {
            return _valueSource.Subscribe(owner, side, value =>
                costSystem.SetDiscount(card, this, value));
        }
    }
}
