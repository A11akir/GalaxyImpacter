using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;
using Feature.HandLogic;
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
        private readonly HeroView _heroView;

        public CreateOwnerCardAndHealthEntitySystem(GameSessionModel gameSessionModel,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository,
            DeckFillSystem deckFillSystem, HandViewSwitcher handViewSwitcher, HandFillSystem handFillSystem, HeroView heroView)
        {
            _gameSessionModel = gameSessionModel;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _deckFillSystem = deckFillSystem;
            _handViewSwitcher = handViewSwitcher;
            _handFillSystem = handFillSystem;
            _heroView = heroView;
        }
        
        public void CreateEntityPlayer(CardAndHealthEntityOwnerData owner)
        {
            _deckFillSystem.InitializeDeck(owner);
            var container = _handViewSwitcher.RegisterOwner(owner);
            _handDataRepository.InitHandRepository(owner, container.HandCardViews);
            _chakraManagerSystem.Init(owner, container.ChakraWindowView);
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);
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

        public void CreatePlayersEntity()
        {
            var playerEntity = _gameSessionModel.PlayerHero.MainHeroEntity();
            CreateEntityPlayer(playerEntity);
            CreateEntityEnemy(_gameSessionModel.EnemyHero.MainHeroEntity());
            _handViewSwitcher.SwitchTo(playerEntity);
        }
    }
}