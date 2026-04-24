using System;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Hero
{
    public class HeroPowerSystem
    {
        private readonly FactoryHandBehaviourTransformCastSystem _factoryHandBehaviourTransformCastSystem;

        private ITransformCastCardBehaviour _heroPowerBehaviour;
        private SpellCardData _heroPower;
        private CardAndHealthEntityOwnerData _owner;
        private GameSessionPlayerData _playerData;
        private CardCastService _cardCastService;
        private GameSessionModel _gameSessionModel;
        
        public event Action OnHeroPowerUsed;    // для игрока
        public event Action OnEnemyHeroPowerUsed;

        public void NotifyEnemyHeroPowerUsed()
        {
            OnEnemyHeroPowerUsed?.Invoke();
        }
        
        public HeroPowerSystem(FactoryHandBehaviourTransformCastSystem factoryHandBehaviourTransformCastSystem,  CardCastService cardCastService, GameSessionModel gameSessionModel)
        {
            _factoryHandBehaviourTransformCastSystem = factoryHandBehaviourTransformCastSystem;
            _cardCastService = cardCastService;
            _gameSessionModel = gameSessionModel;
        }

        public void Init(CardAndHealthEntityOwnerData owner, GameObject heroPowerObject, SpellCardData heroPower,
            GameSessionPlayerData playerData)
        {
            _owner = owner;
            _heroPower = heroPower;
            _playerData = playerData;

            var handCardData = new HandCardData(data: heroPower, view: null, behaviour: null, logic: null);
            _factoryHandBehaviourTransformCastSystem.AddBehavioursToHeroPower(handCardData, heroPowerObject);
            handCardData.Behaviour.SetOwner(owner);
            _heroPowerBehaviour = handCardData.Behaviour;
            _heroPowerBehaviour.CanCastCard(CanCast);
            
            var logic = new HandCardCastHandler(handCardData, _cardCastService);
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
        
        public void ResetAllHeroPowers()
        {
            _playerData.HeroPowerUsedThisTurn = false;
            _heroPowerBehaviour.CanCastCard(CanCast);
            OnHeroPowerUsed?.Invoke(); // обновит вью игрока
    
            _gameSessionModel.EnemyHero.HeroPowerUsedThisTurn = false;
            OnEnemyHeroPowerUsed?.Invoke(); // обновит вью врага
        }

        public void UpdateBehaviour()
        {
            bool canCast = CanCast;
            _heroPowerBehaviour?.CanCastCard(canCast);
        }
    }
}