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
        public void SetCardsPlayerView(List<CardStatsData> cards)
        {
            for (int i = 0; i < _cardsInHand.Count; i++)
            {
                _cardsInHand[i].gameObject.SetActive(false);
                _cardsInHand[i].ClearCardData(_cardsInHand[i]);
            }
            
            for (int i = 0; i < cards.Count; i++)
            {
                _cardsInHand[i].gameObject.SetActive(true);
                _cardsInHand[i].SetDataView(cards[i]);
            }

            UpdateViewCard?.Invoke();
        }
        
        
        
    }
}