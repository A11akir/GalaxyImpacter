// TurnEndEffectPassive.cs — кладёт себя в очередь при OnAppliedFromCard
using System;
using System.Collections.Generic;
using Feature.Card.Script;
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
        private GameSessionData.GameSessionModel _gameSessionModel;
        private Battlefield.Script.BattlefieldSystem _battlefieldSystem;
        private CombatSystem.CombatSystem _combatSystem;
        private CardStatsData _cardData;

        public TurnEndEffectPassive()
        {
            Duration = DurationType.UntilTurnEnd;
        }

// TurnEndEffectPassive.cs
        public void OnAppliedFromCard(EffectContext context)
        {
            Debug.Log($"[TurnEndEffectPassive] OnAppliedFromCard, TurnEndEffectQueue is null={context.TurnEndEffectQueue == null}");

            _caster = context.Caster;
            _target = context.Target;
            _gameSessionModel = context.GameSessionModel;
            _battlefieldSystem = context.BattlefieldSystem;
            _combatSystem = context.CombatSystem;
            _cardData = context.CardData;

            context.TurnEndEffectQueue?.Enqueue(this);
            Debug.Log("[TurnEndEffectPassive] Enqueued");
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