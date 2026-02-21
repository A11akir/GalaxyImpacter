using System;
using System.Collections.Generic;
using Feature.Hero;
using R3;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feature.Card.Script
{
    public class HandCardViews : MonoBehaviour
    {
        [FormerlySerializedAs("_cardsInDeck")] [SerializeField] public List<HandCardView> _cardsInHand;

        public event Action UpdateViewCard;

        public void SetHandCardView(CardStatsData cardStatsData, int index)
        {
            _cardsInHand[index].gameObject.SetActive(false);
            _cardsInHand[index].ClearData();
            _cardsInHand[index].SetDataView(cardStatsData);
            
            UpdateViewCard?.Invoke();
        }
    }
}