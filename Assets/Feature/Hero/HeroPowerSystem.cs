using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Hero
{
    public class HeroPowerSystem
    {
        private readonly FactoryHandBehaviourTransformCastSystem _factoryHandBehaviourTransformCastSystem;

        private readonly List<SpellCardData> _heroPowers = new();
        private readonly List<ITransformCastCardBehaviour> _heroPowerBehaviours = new();
        private CardAndHealthEntityOwnerData _owner;
        private GameSessionPlayerData _playerData;
        private CardCastService _cardCastService;
        private GameSessionModel _gameSessionModel;
        
        public event Action OnHeroPowerUsed;
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

        public void Init(CardAndHealthEntityOwnerData owner, GameObject heroPowerObject,
            SpellCardData heroPower, GameSessionPlayerData playerData, int index) // ← добавить index
        {
            _owner = owner;
            _playerData = playerData;
            _heroPowers.Add(heroPower);

            var handCardData = new HandCardData(data: heroPower, view: null, behaviour: null, logic: null);
            _factoryHandBehaviourTransformCastSystem.AddBehavioursToHeroPower(handCardData, heroPowerObject);
            handCardData.Behaviour.SetOwner(owner);
    
            _heroPowerBehaviours.Add(handCardData.Behaviour);
    
            var currentIndex = index;
            handCardData.Behaviour.CanCastCard(CanCast(currentIndex));
    
            var logic = new HandCardCastHandler(handCardData, _cardCastService);
            handCardData.Behaviour.OnTryCardCast += (o, t) =>
            {
                _playerData.HeroPowerUsage.SetUsed(currentIndex);
                logic.CastCard(o, t);
                OnHeroPowerUsed?.Invoke();
            };
        }
        public bool CanCast(int index) =>
            !_playerData.HeroPowerUsage.IsUsed(index) &&
            _owner.Chakra >= _heroPowers[index].Cost;
        
        public void ResetAllHeroPowers()
        {
            _playerData.HeroPowerUsage.Reset();
    
            for (int i = 0; i < _heroPowerBehaviours.Count; i++)
                _heroPowerBehaviours[i]?.CanCastCard(CanCast(i));
    
            OnHeroPowerUsed?.Invoke();

            _gameSessionModel.EnemyHero.HeroPowerUsage.Reset();
            OnEnemyHeroPowerUsed?.Invoke();
        }

        public void UpdateBehaviour()
        {
            for (int i = 0; i < _heroPowerBehaviours.Count; i++)
                _heroPowerBehaviours[i]?.CanCastCard(CanCast(i));
        }
    }
}