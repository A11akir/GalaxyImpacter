using System;
using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;
using CardEffectBase = Feature.CardEffect.Script.CardEffect;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// При получении урона один раз выполняет вложенный эффект и снимает себя.
    /// Вложенный эффект получает контекст с Caster = владелец, Target = источник урона.
    /// </summary>
    [Serializable]
    public class DamageTriggerPassive : PassiveEffectBase, IDamageReaction, ICardContextConsumer
    {
        [SerializeReference] private CardEffectBase _effect;

        private CardAndHealthEntityOwnerData _owner;
        private GameSessionModel _gameSessionModel;
        private BattlefieldSystem _battlefieldSystem;
        private CombatSystem.CombatSystem _combatSystem;
        private CardPoolPickSystem _cardPoolPickSystem;
        private CritSystem _critSystem;
        private CalculateCardCostSystem _costSystem;
        private GameEventDispatcher _eventDispatcher;
        private SpellCardData _cardData;
        private List<CardEffectBase> _effectsList;
        private bool _isReacting;

        public void OnAppliedFromCard(EffectContext context)
        {
            _gameSessionModel = context.GameSessionModel;
            _battlefieldSystem = context.BattlefieldSystem;
            _combatSystem = context.CombatSystem;
            _cardPoolPickSystem = context.CardPoolPickSystem;
            _critSystem = context.CritSystem;
            _costSystem = context.CostSystem;
            _eventDispatcher = context.EventDispatcher;
            _cardData = context.CardData;
            _effectsList = context.CurrentEffectsList;
        }

        public override void Register(CardAndHealthEntityOwnerData owner) => _owner = owner;

        public override void Unregister() { }

        public int Priority => 95;

        public bool ReactToDamage(CardAndHealthEntityOwnerData target, ref int finalDamage, CardAndHealthEntityOwnerData source)
        {
            if (_isReacting || _effect == null) return false;

            _isReacting = true;

            var innerContext = new EffectContext
            {
                Caster = _owner,
                Target = source,
                Damage = finalDamage,
                GameSessionModel = _gameSessionModel,
                BattlefieldSystem = _battlefieldSystem,
                CombatSystem = _combatSystem,
                CardData = _cardData,
                CardPoolPickSystem = _cardPoolPickSystem,
                CritSystem = _critSystem,
                CostSystem = _costSystem,
                EventDispatcher = _eventDispatcher,
                CurrentEffectsList = _effectsList
            };

            _effect.Execute(innerContext);

            finalDamage = innerContext.Damage;   // эффект мог учесть урон сам (например, выровнять HP)

            target.PassiveEffects.Remove(this);

            _isReacting = false;

            return false;   // урон не блокируется
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new DamageTriggerPassive { _effect = _effect };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
