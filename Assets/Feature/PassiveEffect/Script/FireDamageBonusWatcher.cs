using System;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireDamageBonusWatcher : PassiveEffectBase, IStackablePassive, ICardContextConsumer
    {
        [SerializeField] private PassiveEffectConfig _bonusConfig;

        private int _bonusPerHit;

        private CardAndHealthEntityOwnerData _owner;
        private CombatSystem.CombatSystem _combatSystem;
        private Action<DamageDealtInfo> _handler;

        public void AddBonus(int amount) => _bonusPerHit = amount;

        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem, CardCastService cardCastService, CardPoolPickSystem cardPoolPickSystem)
        {
            _owner = owner;
            _combatSystem = combatSystem;
            _handler = OnDamageDealt;
            combatSystem.OnDamageDealt += _handler;
        }

        public override void Unregister() => _combatSystem.OnDamageDealt -= _handler;

        private void OnDamageDealt(DamageDealtInfo info)
        {
            if (info.Source != _owner) return;
            if (!info.SourceCard) return;
            if (!info.SourceCard.Specialization.Contains(AllHeroClass.FireMage)) return;

            var bonus = _owner.PassiveEffects.Find<FireDamageBonus>();

            if (bonus == null)
            {
                bonus = new FireDamageBonus();
                bonus.SetConfig(_bonusConfig);
                _owner.PassiveEffects.Add(bonus);
            }
            bonus.AddBonus(_bonusPerHit);
        }

        public override PassiveEffectBase Clone() =>
            new FireDamageBonusWatcher { _bonusConfig = _bonusConfig };

        public void OnAppliedFromCard(EffectContext context) =>
            AddBonus(context.CardData.Values[context.ValueIndex]);
    }
}