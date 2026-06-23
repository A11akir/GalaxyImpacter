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
    public class FireDamageBonusWatcher : PassiveEffectBase, IStackablePassive, ICardContextConsumer, IGameEventListener<DamageDealtInfo>
    {
        [SerializeField] private PassiveEffectConfig _bonusConfig;

        private int _bonusPerHit;
        private CardAndHealthEntityOwnerData _owner;

        public void AddBonus(int amount) => _bonusPerHit = amount;

        public void OnAppliedFromCard(EffectContext context) =>
            AddBonus(context.CardData.Values[context.ValueIndex]);

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
        }

        public override void Unregister() { } // ← пусто, нечего отписывать

        public void OnEvent(DamageDealtInfo info)
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
    }
}