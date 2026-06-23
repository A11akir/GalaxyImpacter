// FireSpellOnCastPassive.cs — сохраняем весь нужный набор данных при OnAppliedFromCard
using System;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireSpellOnCastPassive : PassiveEffectBase, ICardContextConsumer
    {
        [SerializeField] private GenerateTemporaryHandCardEffect _generator;

        private CardCastService _castService;
        private CardPoolPickSystem _cardPoolPickSystem;
        private CardAndHealthEntityOwnerData _owner;

        private SpellCardData _sourceCard;
        private int _valueIndex;
        private GameSessionModel _gameSessionModel; // ← добавили

        public void OnAppliedFromCard(EffectContext context)
        {
            _sourceCard = context.CardData;
            _valueIndex = context.ValueIndex;
            _gameSessionModel = context.GameSessionModel; // ← сохраняем
        }

        public override void Register(
            CardAndHealthEntityOwnerData owner,
            CombatSystem.CombatSystem combatSystem,
            CardCastService cardCastService,
            CardPoolPickSystem cardPoolPickSystem)
        {
            _owner = owner;
            _castService = cardCastService;
            _cardPoolPickSystem = cardPoolPickSystem;
            _castService.OnCardCast += OnCardCast;
        }

        public override void Unregister()
        {
            if (_castService != null)
                _castService.OnCardCast -= OnCardCast;
        }

        private void OnCardCast(CardStatsData card, CardAndHealthEntityOwnerData caster)
        {
            if (caster != _owner) return;
            if (card is not SpellCardData) return;
            if (card == _sourceCard) return;

            _generator.Execute(new EffectContext
            {
                Caster = _owner,
                CardData = _sourceCard,
                ValueIndex = _valueIndex,
                CardPoolPickSystem = _cardPoolPickSystem,
                GameSessionModel = _gameSessionModel // ← теперь заполнено
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