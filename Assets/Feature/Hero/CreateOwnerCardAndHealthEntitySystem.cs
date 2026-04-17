using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.Entity.Script;
using Feature.GameSessionData;
using Feature.HandLogic;
using Feature.Health;
using Feature.UI;

namespace Feature.Hero
{
    public class CreateOwnerCardAndHealthEntitySystem
    {
        private readonly HandViewSwitcher _handViewSwitcher;
        private readonly GameSessionModel _gameSessionModel;
        private readonly DeckFillSystem _deckFillSystem;
        private readonly HandDataRepository _handDataRepository;
        private readonly ChakraManagerSystem _chakraManagerSystem;
        private readonly HandFillSystem _handFillSystem;

        private readonly List<EntityPresenter> _entityPresenters = new();
        
        public CreateOwnerCardAndHealthEntitySystem(GameSessionModel gameSessionModel,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository,
            DeckFillSystem deckFillSystem, HandViewSwitcher handViewSwitcher, HandFillSystem handFillSystem)
        {
            _gameSessionModel = gameSessionModel;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _deckFillSystem = deckFillSystem;
            _handViewSwitcher = handViewSwitcher;
            _handFillSystem = handFillSystem;
        }
        


        public void CreateEntityPlayer(CardAndHealthEntityOwnerData owner, IHealthView healthView)
        {
            _deckFillSystem.InitializeDeck(owner);
            var container = _handViewSwitcher.RegisterOwner(owner);
            _handDataRepository.InitHandRepository(owner, container.HandCardViews);
            _chakraManagerSystem.Init(owner, container.ChakraWindowView);
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);
    
            var presenter = new EntityPresenter(owner, healthView);
            _entityPresenters.Add(presenter);
        }

        public void CreatePlayersEntity(IHealthView playerHealthView, IHealthView enemyHealthView)
        {
            var playerEntity = _gameSessionModel.PlayerHero.MainHeroEntity();
            CreateEntityPlayer(playerEntity, playerHealthView);
            /*CreateEntityEnemy(_gameSessionModel.EnemyHero.MainHeroEntity(), enemyHealthView);*/
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
        
    }
}