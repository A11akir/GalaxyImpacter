using System;
using System.Linq;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Сила героя стоит дешевле на велью карты, наложившей эффект.
    /// Карта силы героя задаётся в инспекторе, скидка ставится копии этой карты
    /// у стороны владельца пассивки.
    /// </summary>
    [Serializable]
    public class HeroPowerCostPassive : PassiveEffectBase, ICardContextConsumer
    {
        [SerializeField] private SpellCardData _heroPower;

        private CardAndHealthEntityOwnerData _owner;
        private GameSessionModel _model;
        private CalculateCardCostSystem _costSystem;
        private int _discount;

        public void OnAppliedFromCard(EffectContext context)
        {
            _model = context.GameSessionModel;
            _costSystem = context.CostSystem;
            _discount = context.Value();
        }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;

            var heroPower = GetOwnerHeroPower();
            if (heroPower == null || _costSystem == null) return;

            _costSystem.SetDiscount(heroPower, this, _discount);
        }

        public override void Unregister()
        {
            var heroPower = GetOwnerHeroPower();
            if (heroPower == null || _costSystem == null) return;

            _costSystem.RemoveDiscount(heroPower, this);
        }

        private SpellCardData GetOwnerHeroPower()
        {
            if (_heroPower == null) return null;

            var side = _model?.GetPlayerDataByOwner(_owner);
            if (side?.HeroPowers == null) return _heroPower;

            return side.HeroPowers.FirstOrDefault(power => power.Name == _heroPower.Name) ?? _heroPower;
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new HeroPowerCostPassive { _heroPower = _heroPower };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
