using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Забирает все временные карты из руки кастера и переносит их в руку цели.
    /// </summary>
    [System.Serializable]
    public class TransferTemporaryCardsEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            var caster = context.Caster;
            if (caster == null) return;

            var temporaryCards = caster.CardsInHandList
                .Where(c => c != null && c.Origin == CardOrigin.Temporary)
                .ToList();

            foreach (var owner in ResolveTargets(context))
            {
                if (owner == null || owner == caster) continue;

                foreach (var card in temporaryCards)
                {
                    caster.RemoveCardFromHand(card);
                    owner.AddCardToHand(card, owner.CountCardsInHand);
                }
            }
        }
    }
}