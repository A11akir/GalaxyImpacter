using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class TurnEndEffectPassive : PassiveEffectBase, ICardContextConsumer
    {
        [SerializeReference] private List<CardEffect> _effects = new();

        private CardAndHealthEntityOwnerData _caster;
        private CardAndHealthEntityOwnerData _target;
        private GameSessionModel _gameSessionModel;
        private Battlefield.Script.BattlefieldSystem _battlefieldSystem;
        private CombatSystem.CombatSystem _combatSystem;
        private CardStatsData _cardData;

        public TurnEndEffectPassive() => Duration = DurationType.UntilTurnEnd;

        public void OnAppliedFromCard(EffectContext context)
        {
            _caster = context.Caster;
            _target = context.Target;
            _gameSessionModel = context.GameSessionModel;
            _battlefieldSystem = context.BattlefieldSystem;
            _combatSystem = context.CombatSystem;
            _cardData = context.CardData;

            context.TurnEndEffectQueue?.Enqueue(this);
        }

        public override void Register(CardAndHealthEntityOwnerData owner) { }
        public override void Unregister() { }

        public void TriggerEffects()
        {
            var innerContext = new EffectContext
            {
                Caster = _caster,
                Target = _target,
                GameSessionModel = _gameSessionModel,
                BattlefieldSystem = _battlefieldSystem,
                CombatSystem = _combatSystem,
                CardData = (SpellCardData)_cardData,
                CurrentEffectsList = _effects,
            };

            for (int i = 0; i < _effects.Count; i++)
            {
                innerContext.ValueIndex = i;
                _effects[i].Execute(innerContext);
            }
        }

        public override PassiveEffectBase Clone() =>
            new TurnEndEffectPassive { _effects = _effects, Config = Config };
    }
}