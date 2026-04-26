using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;
using Feature.Hero;
using Feature.ShopGamePlay.Script.Currency;

namespace Feature.StagesGameLogic
{
    public class TurnResourceManager
    {
        private readonly ChakraManagerSystem _chakraManager;
        private readonly CurrencyManagerSystem _currencyManager;
        private readonly HeroPowerSystem _heroPowerSystem;
        private readonly HandFillSystem _handFillSystem;
        private readonly GameSessionModel _gameSessionModel;

        public TurnResourceManager(ChakraManagerSystem chakraManager, CurrencyManagerSystem currencyManager, HeroPowerSystem heroPowerSystem, HandFillSystem handFillSystem, GameSessionModel gameSessionModel)
        {
            _chakraManager = chakraManager;
            _currencyManager = currencyManager;
            _heroPowerSystem = heroPowerSystem;
            _handFillSystem = handFillSystem;
            _gameSessionModel = gameSessionModel;
        }

        public void StartNewTurn()
        {
            _heroPowerSystem.ResetAllHeroPowers();
        
            foreach (var owner in _gameSessionModel.GetAllEntityOwners())
                owner.DiscardHand();
        
            _handFillSystem.FillHandDataInDecks();
            _chakraManager.NewTurnUpdate();
            _currencyManager.NewTurnUpdate();
        }
    }
}