using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.GameSessionData;
using Feature.ShopGamePlay.Script;
using Feature.ShopGamePlay.Script.Currency;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class TurnСycleGameSessionSystem
    {
        private HandCardPresenter _handCardPresenter;
        private HandFillSystem _handFillSystem;
        private BattlefieldSystem _battlefieldSystem;
        private DeckFillSystem _deckFillSystem { get;  }
        private HandDataRepository _handDataRepository { get;  }
        private CurrencyManagerSystem _currencyManagerSystem { get;  }        
        private ChakraManagerSystem _chakraManagerSystem { get;  }
        
        public TurnСycleGameSessionSystem(DeckFillSystem deckFillSystem, CurrencyManagerSystem currencyManagerSystem,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository, BattlefieldSystem battlefieldSystem, HandFillSystem handFillSystem, HandCardPresenter handCardPresenter)
        {
            _deckFillSystem = deckFillSystem;
            _currencyManagerSystem = currencyManagerSystem;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _battlefieldSystem = battlefieldSystem;
            _handFillSystem = handFillSystem;
            _handCardPresenter = handCardPresenter;
        }
        

        public void StartGameSession()
        {
            _deckFillSystem.InitializeDecks();
            _battlefieldSystem.Init();
            _currencyManagerSystem.Init();
            _chakraManagerSystem.Init();
            _handDataRepository.Init();
        }

        public void CycleTurn()
        {
            _handFillSystem.FillHandDataInDecks();
            _handDataRepository.InitPropertyCard(); // TODO: сделать добавление на каждую карту отдельно а не всю руку
            _handCardPresenter.SetCardInPlayerHand();

            _currencyManagerSystem.NewTurnUpdate();
            _chakraManagerSystem.NewTurnUpdate();
        }
    }
}