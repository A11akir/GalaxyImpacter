using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Hero
{
    public class HeroPowerSystem
    {
        private HeroPowerView _heroPowerView;
        private readonly CardCastSystem _cardCastSystem;
        private readonly GameSessionModel _gameSessionModel;
        private readonly BattlefieldSystem _battlefieldSystem;
        private readonly CombatSystem.CombatSystem _combatSystem;
        private ITransformCastCardBehaviour _heroPowerBehaviour;
        private SpellCardData _heroPower;
        private CardAndHealthEntityOwnerData _owner;
        private GameSessionPlayerData _playerData;

        public HeroPowerSystem(CardCastSystem cardCastSystem, GameSessionModel gameSessionModel,
            BattlefieldSystem battlefieldSystem, CombatSystem.CombatSystem combatSystem)
        {
            _cardCastSystem = cardCastSystem;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
            _combatSystem = combatSystem;
        }

        public void Init(CardAndHealthEntityOwnerData owner, GameObject heroPowerObject, SpellCardData heroPower, GameSessionPlayerData playerData)
        {
            _owner = owner;
            _heroPower = heroPower;
            _playerData = playerData;
            _heroPowerView = heroPowerObject.GetComponent<HeroPowerView>();

            var handCardData = new HandCardData(data: heroPower, view: null, behaviour: null, logic: null);
            _cardCastSystem.AddBehavioursToHeroPower(handCardData, heroPowerObject);
            handCardData.Behaviour.SetOwner(owner);
    
            bool canCast = owner.Chakra >= heroPower.Cost;
            handCardData.Behaviour.CanCastCard(canCast);
            _heroPowerView?.SetCanCastView(canCast);

            _heroPowerBehaviour = handCardData.Behaviour;

            var logic = new GameplayLogicCard(handCardData, _gameSessionModel, _battlefieldSystem, _combatSystem);
            handCardData.Behaviour.OnTryCardCast += (o, t) =>
            {
                _playerData.HeroPowerUsedThisTurn = true;
                handCardData.Behaviour.CanCastCard(false);
                _heroPowerView?.SetCanCastView(false);
                logic.CastCard(o, t);
            };
        }

        public void ResetHeroPower()
        {
            _playerData.HeroPowerUsedThisTurn = false;
            UpdateCanCastView();
        }

        public void UpdateCanCastView()
        {
            if (_heroPowerBehaviour == null || _owner == null || !_heroPower) return;
            bool canCast = !_playerData.HeroPowerUsedThisTurn && _owner.Chakra >= _heroPower.Cost;
            _heroPowerBehaviour.CanCastCard(canCast);
            _heroPowerView?.SetCanCastView(canCast);
        }
    }
}