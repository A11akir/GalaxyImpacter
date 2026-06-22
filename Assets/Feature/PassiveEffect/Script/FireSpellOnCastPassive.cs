using System;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class FireSpellOnCastPassive : PassiveEffectBase
    {
        [SerializeField] private GenerateTemporaryHandCardEffect _generator;

        private CardCastService _castService;
        private CardPoolPickSystem _cardPoolPickSystem;
        private CardAndHealthEntityOwnerData _owner;

        public override void InjectServices(
            CardCastService castService,
            CardPoolPickSystem cardPoolPickSystem)
        {
            _castService = castService;
            _cardPoolPickSystem = cardPoolPickSystem;
        }

        public override void Register(
            CardAndHealthEntityOwnerData owner,
            CombatSystem.CombatSystem combatSystem)
        {
            _owner = owner;

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

            _generator.Execute(new EffectContext
            {
                Caster = _owner,
                Target = null,
                GameSessionModel = null,
                BattlefieldSystem = null,
                CombatSystem = null,
                CardData = new SpellCardData(), // заглушка ок
                CardPoolPickSystem = _cardPoolPickSystem,
                ValueIndex = 0,
                CurrentEffectsList = null
            });
        }

        public override PassiveEffectBase Clone()
        {
            return new FireSpellOnCastPassive
            {
                Config = Config,
                Duration = Duration
            };
        }
    }
}