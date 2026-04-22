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
        private readonly HeroPowerPresenter _heroPowerPresenter;

        private readonly Dictionary<CardAndHealthEntityOwnerData, EntityPresenter> _entityPresenters = new();
        
        public CreateOwnerCardAndHealthEntitySystem(GameSessionModel gameSessionModel,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository,
            DeckFillSystem deckFillSystem, HandViewSwitcher handViewSwitcher, HandFillSystem handFillSystem,
            EntityDeathSystem entityDeathSystem, HeroPowerSystem heroPowerSystem, HeroPowerPresenter heroPowerPresenter)
        {
            _gameSessionModel = gameSessionModel;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _deckFillSystem = deckFillSystem;
            _handViewSwitcher = handViewSwitcher;
            _handFillSystem = handFillSystem;
            _entityDeathSystem = entityDeathSystem;
            _heroPowerSystem = heroPowerSystem;
            _heroPowerPresenter = heroPowerPresenter;
            _entityDeathSystem.OnEntityDied += DisposeEntity;
        }
        

        public void CreatePlayersEntity(IHealthView playerHealthView, IHealthView enemyHealthView, 
            HeroPowerView heroPowerView, SpellCardData heroPower, HandCardViews enemyHandCardViews)
        {
            var playerEntity = _gameSessionModel.PlayerHero.MainHeroEntity();
            CreateEntityPlayer(playerEntity, playerHealthView);
            _handViewSwitcher.SwitchTo(playerEntity);
            _heroPowerSystem.Init(playerEntity, heroPowerView.gameObject, heroPower, _gameSessionModel.PlayerHero);
            _heroPowerPresenter.Init(heroPowerView);

            CreateMainEnemyEntity(_gameSessionModel.EnemyHero.MainHeroEntity(), enemyHealthView, enemyHandCardViews);
        }
        
        public void CreateEntityPlayer(CardAndHealthEntityOwnerData owner, IHealthView healthView)
        {
            _deckFillSystem.InitializeDeck(owner);
            var container = _handViewSwitcher.RegisterOwner(owner);
            _handDataRepository.InitHandRepository(owner, container.HandCardViews);
            _chakraManagerSystem.Init(owner, container.ChakraWindowView);
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);
            _entityDeathSystem.Init(owner);

            var presenter = new EntityPresenter(owner, healthView);
            _entityPresenters[owner] = presenter;
        }

        private void CreateMainEnemyEntity(CardAndHealthEntityOwnerData owner, IHealthView healthView, HandCardViews handCardViews)
        {
            _deckFillSystem.InitializeDeck(owner);
            _handDataRepository.InitHandRepository(owner, handCardViews, isHidden: true);
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);
            _entityDeathSystem.Init(owner);
            _gameSessionModel.EnemyHero.InitBoard();

            var presenter = new EntityPresenter(owner, healthView);
            _entityPresenters[owner] = presenter;
        }
        public void CreateEntityEnemy(CardAndHealthEntityOwnerData owner, IHealthView healthView)
        {
            _deckFillSystem.InitializeDeck(owner);
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);
            _entityDeathSystem.Init(owner);

            var presenter = new EntityPresenter(owner, healthView);
            _entityPresenters[owner] = presenter;
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