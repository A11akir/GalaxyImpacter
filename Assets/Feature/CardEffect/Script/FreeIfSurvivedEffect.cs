using System;
using System.Linq;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Карта бесплатна, пока требуемое существо выжило >= N ходов, где N = Values[0] карты.
    /// </summary>
    [Serializable]
    public class FreeIfSurvivedEffect : PassiveCardEffect
    {
        [SerializeReference] private IDynamicCostValueSource _surviveSource;

        public override IDisposable BindPassiveEffect(
            CardAndHealthEntityOwnerData owner,
            GameSessionPlayerData side,
            CardStatsData card,
            CalculateCardCostSystem costSystem,
            CardCastService castService)
        {
            var values = (card as SpellCardData)?.Values;
            int required = values != null && values.Count > 0 ? values[^1] : 0;

            return _surviveSource.Subscribe(owner, side, survived =>
                costSystem.SetDiscount(card, this, survived >= required ? card.BaseCost : 0));
        }
    }
}
