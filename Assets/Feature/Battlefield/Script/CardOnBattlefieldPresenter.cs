using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Battlefield.Script
{
    public class CardOnBattlefieldPresenter
    {
        public void SetCardInPlayerHand(CardOnBattlefieldView view, CardStatsData cardStatsData)
        {
            Debug.Log("SetCardInPlayerHand");
            Debug.Log(cardStatsData.Name);
            Debug.Log(view.ToString());
            view.SetDataView(cardStatsData);
        }
    }
}