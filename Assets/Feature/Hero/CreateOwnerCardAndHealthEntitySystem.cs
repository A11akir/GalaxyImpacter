using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;

namespace Feature.Hero
{
    public class CreateOwnerCardAndHealthEntitySystem
    {
        GameSessionModel _gameSessionModel;
        DeckFillSystem _deckFillSystem;
        HandDataRepository _handDataRepository;
        ChakraManagerSystem _chakraManagerSystem;

        public CreateOwnerCardAndHealthEntitySystem(GameSessionModel gameSessionModel, ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository, DeckFillSystem deckFillSystem)
        {
            _gameSessionModel = gameSessionModel;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _deckFillSystem = deckFillSystem;
        }

        public void CreateEntity(CardAndHealthEntityOwnerData cardAndHealthEntityOwnerData)
        {
            _deckFillSystem.InitializeDeck(cardAndHealthEntityOwnerData);
            _handDataRepository.InitHandRepository(cardAndHealthEntityOwnerData);
            _chakraManagerSystem.Init(cardAndHealthEntityOwnerData);
        }

        public void CreatePlayersEntity()
        {
            CreateEntity(_gameSessionModel.PlayerHero.MainHeroEntity());
            CreateEntity(_gameSessionModel.EnemyHero.MainHeroEntity());
        }
    }
}