using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AttackCostPassive : PassiveEffectBase, ICardContextConsumer
    {
        private CardAndHealthEntityOwnerData _owner;
        private IDisposable _subscription;
        private CalculateCardCostSystem _costSystem;
        private int _discount;

        public void OnAppliedFromCard(EffectContext context)
        {
            _discount = context.Value();
            _costSystem = context.CostSystem;
        }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
            _subscription = owner.CardsInHand.Subscribe(ApplyDiscount);
        }

        public override void Unregister()
        {
            _subscription?.Dispose();
            _subscription = null;

            if (_costSystem == null) return;

            foreach (var card in _owner.CardsInHand.CurrentValue)
            {
                if (card == null) continue;
                _costSystem.RemoveDiscount(card, this);
            }
        }

        private void ApplyDiscount(List<CardStatsData> cards)
        {
            foreach (var card in cards)
            {
                if (card == null || !DealsDamage(card)) continue;

                _costSystem.SetDiscount(card, this, _discount);
            }
        }

        private static bool DealsDamage(CardStatsData card) =>
            card is SpellCardData spell && spell.Effects.Any(e => e is DamageEffect);

        public override PassiveEffectBase Clone()
        {
            var clone = new AttackCostPassive();
            CopyBaseFields(clone);
            return clone;
        }
    }
}
