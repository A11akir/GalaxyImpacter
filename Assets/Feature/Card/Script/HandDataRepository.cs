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
        private Dictionary<int, HandCardData> _cachedHandData = new();
        
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

            _cardCastSystem.AddBehavioursToCards(_handData);
            BindBehavioursToLogic();
            _handCardPresenter.SetCardInPlayerHand();
            
        }

        private void BindBehavioursToLogic()
        {
            foreach (var card in _handData)
            {

                Debug.Log("BindBehavioursToLogic");

                card.Logic = new GameplayLogicCard(card, _gameSessionModel, _battlefieldSystem);
                
                if (card.OnTryCardCastHandler != null)
                {
                    card.Behaviour.OnTryCardCast -= card.OnTryCardCastHandler;
                }
                
                card.OnTryCardCastHandler = () => card.Logic.CastCard();
                
                card.Behaviour.OnTryCardCast += card.OnTryCardCastHandler;
                
            }
        }
        
        private List<HandCardData> CombineDataAndViews(List<CardStatsData> dataList, List<HandCardView> viewList)
        {
            var result = new List<HandCardData>();
            int count = Mathf.Min(dataList.Count, viewList.Count);

            for (int i = 0; i < count; i++)
            {
                if (!_cachedHandData.TryGetValue(i, out var handCardData))
                {
                    handCardData = new HandCardData(i, dataList[i], viewList[i], null, null);
                    _cachedHandData[i] = handCardData;
                }
                else
                {
                    handCardData.Data = dataList[i];
                    handCardData.View = viewList[i];
                }
                
                if (handCardData.Behaviour != null && handCardData.Behaviour is Component comp)
                {
                    Object.Destroy(comp);
                    handCardData.Behaviour = null;
                }
                
                _cardCastSystem.AddBehavioursToCards(new List<HandCardData> { handCardData });

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