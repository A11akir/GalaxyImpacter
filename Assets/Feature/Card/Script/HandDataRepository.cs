using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandDataRepository
    {
        public List<HandCardData> _handData = new List<HandCardData>();
        
        
        private BattlefieldSystem _battlefieldSystem;
        private GameSessionModel _gameSessionModel;
        private HandFillSystem _handFillSystem;
        private HandCardPresenter _handCardPresenter;
        private CardCastSystem _cardCastSystem;

        public HandDataRepository(
            CardCastSystem cardCastSystem, 
            HandCardPresenter handCardPresenter, 
            HandFillSystem handFillSystem, GameSessionModel gameSessionModel, BattlefieldSystem battlefieldSystem)
        {
            _cardCastSystem = cardCastSystem;
            _handCardPresenter = handCardPresenter;
            _handFillSystem = handFillSystem;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
        }

        public void Init()
        {
            Debug.Log("InitHandDataRepository");
            _gameSessionModel.PlayerHero.CardsInHand
                .Select(x => x.Count)
                .DistinctUntilChanged()
                .Subscribe(_ => UpdateHandCard());
        }

        public void InitPropertyCard()
        {
            Debug.Log("InitPropertyCard");
            var dataList = _handFillSystem.GetHandData();
            var viewList = _handCardPresenter.GetHandViews();
            _handData = CombineDataAndViews(dataList, viewList);
            
            _cardCastSystem.AddBehavioursToCards(_handData);
            BindBehavioursToLogic();
        }

        private void UpdateHandCard()
        {
            var dataList = _handFillSystem.GetHandData();
            var viewList = _handCardPresenter.GetHandViews();
            _handData = CombineDataAndViews(dataList, viewList);

            _handCardPresenter.SetCardInPlayerHand();
        }

        private void BindBehavioursToLogic()
        {
            foreach (var card in _handData)
            {
                if (card.IsLogicInitialized)
                    continue;

                Debug.Log("BindBehavioursToLogic");
                card.Logic = new GameplayLogicCard(card, _gameSessionModel, _battlefieldSystem);

                var cachedCard = card;

                card.Behaviour.OnTryCardCast += () =>
                {
                    cachedCard.Logic.CastCard();

                };

                card.IsLogicInitialized = true;
            }
        }
        
        private List<HandCardData> CombineDataAndViews(
            List<CardStatsData> dataList, 
            List<HandCardView> viewList)
        {
            var result = new List<HandCardData>();
            
            int count = Mathf.Min(dataList.Count, viewList.Count);
            
            for (int i = 0; i < count; i++)
            {
                var handCardData = new HandCardData(
                    index: i,
                    data: dataList[i],
                    view: viewList[i],
                    behaviour: null ,
                    logic: null
                );
                
                result.Add(handCardData);
            }
            
            return result;
        }
        
        public HandCardData GetHandCardByIndex(int index)
        {
            if (index >= 0 && index < _handData.Count)
                return _handData[index];
                
            return null;
        }
    }
}