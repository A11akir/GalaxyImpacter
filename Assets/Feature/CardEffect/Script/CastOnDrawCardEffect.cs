using System;
using Feature.Card.Script;
using Feature.GameSessionData;
using R3;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Карта разыгрывается сама, как только попадает в руку. Чакра тратится как за обычный каст
    /// (CardCastService.Cast сам проверяет хватает ли чакры — если нет, карта остаётся в руке).
    /// </summary>
    [Serializable]
    public class CastOnDrawCardEffect : PassiveCardEffect
    {
        public override IDisposable BindPassiveEffect(
            CardAndHealthEntityOwnerData owner,
            GameSessionPlayerData side,
            CardStatsData card,
            CalculateCardCostSystem costSystem,
            CardCastService castService)
        {
            castService.Cast(card, owner, null);

            return Disposable.Empty;
        }
    }
}
