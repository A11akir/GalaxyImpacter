using System;
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
        private ITransformCastCardBehaviour _heroPowerBehaviour;
        private SpellCardData _heroPower;
        private CardAndHealthEntityOwnerData _owner;
        private GameSessionPlayerData _playerData;


        
        public event Action OnHeroPowerUsed;

        public HeroPowerSystem(CardCastSystem cardCastSystem, GameSessionModel gameSessionModel,
            BattlefieldSystem battlefieldSystem, CombatSystem.CombatSystem combatSystem)
        {
            _cardCastSystem = cardCastSystem;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
            _combatSystem = combatSystem;
        }

        public void Init(CardAndHealthEntityOwnerData owner, GameObject heroPowerObject, SpellCardData heroPower,
            GameSessionPlayerData playerData)
        {
            _owner = owner;
            _heroPower = heroPower;
            _playerData = playerData;

            var handCardData = new HandCardData(data: heroPower, view: null, behaviour: null, logic: null);
            _cardCastSystem.AddBehavioursToHeroPower(handCardData, heroPowerObject);
            handCardData.Behaviour.SetOwner(owner);
            _heroPowerBehaviour = handCardData.Behaviour;

            var logic = new GameplayLogicCard(handCardData, _gameSessionModel, _battlefieldSystem, _combatSystem);
            handCardData.Behaviour.OnTryCardCast += (o, t) =>
            {
                _playerData.HeroPowerUsedThisTurn = true;
                logic.CastCard(o, t);
                OnHeroPowerUsed?.Invoke();
            };
        }


        public bool CanCast => !_playerData.HeroPowerUsedThisTurn && _owner.Chakra >= _heroPower.Cost;
        
        public void ResetHeroPower()
        {
            _playerData.HeroPowerUsedThisTurn = false;
            _heroPowerBehaviour.CanCastCard(CanCast);
        }

        public void UpdateBehaviour() => _heroPowerBehaviour?.CanCastCard(CanCast);
    }
}