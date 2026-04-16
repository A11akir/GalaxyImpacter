using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;
using Feature.HandLogic;

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

        public CreateOwnerCardAndHealthEntitySystem(GameSessionModel gameSessionModel, ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository, DeckFillSystem deckFillSystem, HandViewSwitcher handViewSwitcher, HandFillSystem handFillSystem)
        {
            _gameSessionModel = gameSessionModel;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _deckFillSystem = deckFillSystem;
            _handViewSwitcher = handViewSwitcher;
            _handFillSystem = handFillSystem;
        }

        public void CreateEntityPlayer(CardAndHealthEntityOwnerData cardAndHealthEntityOwnerData)
        {
            _deckFillSystem.InitializeDeck(cardAndHealthEntityOwnerData);
            _handDataRepository.InitHandRepository(cardAndHealthEntityOwnerData);
            _chakraManagerSystem.Init(cardAndHealthEntityOwnerData);
            _handViewSwitcher.RegisterOwner(cardAndHealthEntityOwnerData);
            _handFillSystem.FillEntityHand(cardAndHealthEntityOwnerData);
            _chakraManagerSystem.InitEntityChakra(cardAndHealthEntityOwnerData);
        }

        private void CreateEntityEnemy(CardAndHealthEntityOwnerData cardAndHealthEntityOwnerData)
        {
            _deckFillSystem.InitializeDeck(cardAndHealthEntityOwnerData);
            _handDataRepository.InitHandRepository(cardAndHealthEntityOwnerData);
            _chakraManagerSystem.Init(cardAndHealthEntityOwnerData);
            _handFillSystem.FillEntityHand(cardAndHealthEntityOwnerData);
            _chakraManagerSystem.InitEntityChakra(cardAndHealthEntityOwnerData);
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