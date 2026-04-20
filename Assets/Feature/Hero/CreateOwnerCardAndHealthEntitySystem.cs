using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.Entity.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.HandLogic;
using Feature.Health;
using Feature.UI;
using UnityEngine;

namespace Feature.Hero
{
    public class CreateOwnerCardAndHealthEntitySystem
    {
        private readonly HandViewSwitcher _handViewSwitcher;
        private readonly GameSessionModel _gameSessionModel;
        private readonly DeckFillSystem _deckFillSystem;
        private readonly HandDataRepository _handDataRepository;
        private readonly EntityDeathSystem _entityDeathSystem;
        private readonly ChakraManagerSystem _chakraManagerSystem;
        private readonly HandFillSystem _handFillSystem;
        private readonly HeroPowerSystem _heroPowerSystem;

        private readonly Dictionary<CardAndHealthEntityOwnerData, EntityPresenter> _entityPresenters = new();
        
        public CreateOwnerCardAndHealthEntitySystem(GameSessionModel gameSessionModel,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository,
            DeckFillSystem deckFillSystem, HandViewSwitcher handViewSwitcher, HandFillSystem handFillSystem,
            EntityDeathSystem entityDeathSystem, HeroPowerSystem heroPowerSystem)
        {
            _gameSessionModel = gameSessionModel;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _deckFillSystem = deckFillSystem;
            _handViewSwitcher = handViewSwitcher;
            _handFillSystem = handFillSystem;
            _entityDeathSystem = entityDeathSystem;
            _heroPowerSystem = heroPowerSystem;
            _entityDeathSystem.OnEntityDied += DisposeEntity;
        }
        
        public void CreateEntityPlayer(CardAndHealthEntityOwnerData owner, IHealthView healthView, HeroPowerView heroPowerView, SpellCardData heroPower)
        {
            _deckFillSystem.InitializeDeck(owner);
            var container = _handViewSwitcher.RegisterOwner(owner);
            _handDataRepository.InitHandRepository(owner, container.HandCardViews);
            _chakraManagerSystem.Init(owner, container.ChakraWindowView);
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);
            _entityDeathSystem.Init(owner);
            _heroPowerSystem.Init(owner, heroPowerView.gameObject, heroPower);

            var presenter = new EntityPresenter(owner, healthView);
            _entityPresenters[owner] = presenter;
        }

        public void CreatePlayersEntity(IHealthView playerHealthView, IHealthView enemyHealthView, 
            HeroPowerView heroPowerView, SpellCardData heroPower)
        {
            var playerEntity = _gameSessionModel.PlayerHero.MainHeroEntity();
            CreateEntityPlayer(playerEntity, playerHealthView, heroPowerView, heroPower);
            _handViewSwitcher.SwitchTo(playerEntity);
        }

        private void CreateEntityEnemy(CardAndHealthEntityOwnerData cardAndHealthEntityOwnerData)
        {
            /*_deckFillSystem.InitializeDeck(owner);
            var container = _handViewSwitcher.RegisterOwner(owner); // получаем контейнер
            _handDataRepository.InitHandRepository(owner, container.HandCardViews); // передаём
            _chakraManagerSystem.Init(owner, container.ChakraWindowView); // передаём вьюху чакры
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);*/
        }
        
        public void DisposeEntity(CardAndHealthEntityOwnerData owner)
        {
            if (_entityPresenters.TryGetValue(owner, out var presenter))
            {
                presenter.Dispose();
                _entityPresenters.Remove(owner);
            }
        }
        
    }
}