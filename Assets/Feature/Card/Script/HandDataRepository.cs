using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandDataRepository
    {
        public List<HandCardData> _handData = new List<HandCardData>();
        
        private HandFillSystem _handFillSystem;
        private HandCardPresenter _handCardPresenter;
        private CardCastSystem _cardCastSystem;

        public HandDataRepository(
            CardCastSystem cardCastSystem, 
            HandCardPresenter handCardPresenter, 
            HandFillSystem handFillSystem)
        {
            _cardCastSystem = cardCastSystem;
            _handCardPresenter = handCardPresenter;
            _handFillSystem = handFillSystem;
        }

        public void InitPropertyCard()
        {
            var dataList = _handFillSystem.GetHandData();
            var viewList = _handCardPresenter.GetHandViews();
            _handCardPresenter.SetCardInPlayerHand();
            _handData = CombineDataAndViews(dataList, viewList);
            
            _cardCastSystem.AddBehavioursToCards(_handData);
        }
        
        private List<HandCardData> CombineDataAndViews(
            List<CardStatsData> dataList, 
            List<CardView> viewList)
        {
            var result = new List<HandCardData>();
            
            int count = Mathf.Min(dataList.Count, viewList.Count);
            
            for (int i = 0; i < count; i++)
            {
                var handCardData = new HandCardData(
                    index: i,
                    data: dataList[i],
                    view: viewList[i],
                    behaviour: null 
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