using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;
using R3;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class TurnEndEffectPassive : PassiveEffectBase, IValueProvider, IStackablePassive
    {
        private int stackCount;
        [SerializeReference] private List<CardEffect> _effects = new();

        private readonly ReactiveProperty<int> _value = new(0);

        public ReadOnlyReactiveProperty<int> Value => _value;
        private CardAndHealthEntityOwnerData _caster;
        private CardAndHealthEntityOwnerData _target;
        private GameSessionData.GameSessionModel _gameSessionModel;
        private Battlefield.Script.BattlefieldSystem _battlefieldSystem;
        private CombatSystem.CombatSystem _combatSystem;
        private CardStatsData _cardData;


        public TurnEndEffectPassive()
        {
            Duration = DurationType.UntilTurnEnd;
        }

        public void Setup(EffectContext sourceContext)
        {
            _caster = sourceContext.Caster;
            _target = sourceContext.Target;
            _gameSessionModel = sourceContext.GameSessionModel;
            _battlefieldSystem = sourceContext.BattlefieldSystem;
            _combatSystem = sourceContext.CombatSystem;
            _cardData = sourceContext.CardData;
        }

        public override void Register(CardAndHealthEntityOwnerData owner, CombatSystem.CombatSystem combatSystem) { }
        public override void Unregister() { }


        protected override void OnTurnTick()
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
                Debug.Log($"[TurnEndEffectPassive] Executing effect[{i}] = {_effects[i].GetType().Name}");
                innerContext.ValueIndex = i;
                _effects[i].Execute(innerContext);
            }
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new TurnEndEffectPassive
            {
                _effects = _effects,
                Config = Config
            };
            return clone;
        }

        public void AddBonus(int amount)
        {
            stackCount += amount;
            _value.Value = stackCount;
        }
    }
}