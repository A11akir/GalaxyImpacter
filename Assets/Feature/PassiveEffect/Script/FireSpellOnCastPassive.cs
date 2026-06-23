// FireSpellOnCastPassive.cs — полностью на новой модели
using System;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireSpellOnCastPassive : PassiveEffectBase, ICardContextConsumer, IGameEventListener<CardCastInfo>
    {
        [SerializeField] private GenerateTemporaryHandCardEffect _generator;

        private CardAndHealthEntityOwnerData _owner;
        private SpellCardData _sourceCard;
        private int _valueIndex;
        private GameSessionModel _gameSessionModel;
        private CardPoolPickSystem _cardPoolPickSystem;

        public void OnAppliedFromCard(EffectContext context)
        {
            _sourceCard = context.CardData;
            _valueIndex = context.ValueIndex;
            _gameSessionModel = context.GameSessionModel;
            _cardPoolPickSystem = context.CardPoolPickSystem; // ← тоже берём отсюда, не из Register
        }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
        }

        public override void Unregister() { }

        public void OnEvent(CardCastInfo info)
        {
            if (info.Caster != _owner) return;
            if (info.Card is not SpellCardData) return;
            if (info.Card == _sourceCard) return;

            _generator.Execute(new EffectContext
            {
                Caster = _owner,
                CardData = _sourceCard,
                ValueIndex = _valueIndex,
                CardPoolPickSystem = _cardPoolPickSystem,
                GameSessionModel = _gameSessionModel
            });
        }

        public override PassiveEffectBase Clone() =>
            new FireSpellOnCastPassive
            {
                Config = Config,
                Duration = Duration,
                _generator = _generator
            };
    }
}