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
        private readonly FactoryHandBehaviourTransformCastSystem _factoryHandBehaviourTransformCastSystem;

        private ITransformCastCardBehaviour _heroPowerBehaviour;
        private SpellCardData _heroPower;
        private CardAndHealthEntityOwnerData _owner;
        private GameSessionPlayerData _playerData;
        private CardCastService _cardCastService;
        
        public event Action OnHeroPowerUsed;
        public bool IsUsedThisTurn => _playerData.HeroPowerUsedThisTurn;

        public HeroPowerSystem(FactoryHandBehaviourTransformCastSystem factoryHandBehaviourTransformCastSystem,  CardCastService cardCastService)
        {
            _factoryHandBehaviourTransformCastSystem = factoryHandBehaviourTransformCastSystem;
            _cardCastService = cardCastService;
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

        public void UpdateBehaviour() => _heroPowerBehaviour?.CanCastCard(CanCast);
    }
}