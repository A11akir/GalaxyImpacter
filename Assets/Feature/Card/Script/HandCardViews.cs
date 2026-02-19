using System;
using System.Collections.Generic;
using Feature.Hero;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandCardViews : MonoBehaviour
    {
        [SerializeField] public List<CardView> _cardsInDeck;

        public event Action UpdateViewCard;
        public void SetCardsPlayerView(List<CardStatsData> playerHeroCardsInDeck)
        {
            for (int i = 0; i < playerHeroCardsInDeck.Count; i++)
            {
                _cardsInDeck[i].SetDataView(playerHeroCardsInDeck[i]);
            }
            UpdateViewCard?.Invoke();
        }
        
    }
}