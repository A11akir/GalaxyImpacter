using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Hero
{
    public class HeroPowerSystem
    {
        private readonly CardCastSystem _cardCastSystem;
        private readonly GameSessionModel _gameSessionModel;
        private readonly BattlefieldSystem _battlefieldSystem;
        private readonly CombatSystem.CombatSystem _combatSystem;

        public HeroPowerSystem(CardCastSystem cardCastSystem, GameSessionModel gameSessionModel,
            BattlefieldSystem battlefieldSystem, CombatSystem.CombatSystem combatSystem)
        {
            _cardCastSystem = cardCastSystem;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
            _combatSystem = combatSystem;
        }

        public void Init(CardAndHealthEntityOwnerData owner, HeroPowerView heroPowerView, SpellCardData heroPower)
        {
            var handCardData = new HandCardData(
                data: heroPower,
                view: heroPowerView,
                behaviour: null,
                logic: null);

            _cardCastSystem.AddBehavioursToCard(handCardData, isHeroPower: true);
            handCardData.Behaviour.SetOwner(owner);
            handCardData.Behaviour.CanCastCard(owner.Chakra >= heroPower.Cost);

            var logic = new GameplayLogicCard(handCardData, _gameSessionModel, _battlefieldSystem, _combatSystem);
            handCardData.Behaviour.OnTryCardCast += logic.CastCard;
        }
    }
}