using System;
using System.Collections.Generic;
using Feature.Hero;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandCardViews : MonoBehaviour
    {
        [SerializeField] private List<CardView> _cardsInDeck;

        public event Action UpdateViewCard;
        public void SetCardsPalyerView(List<CardStatsData> playerHeroCardsInDeck)
        {
            for (int i = 0; i < playerHeroCardsInDeck.Count; i++)
            {
                _cardsInDeck[i].SetDataView(playerHeroCardsInDeck[i]);
            }
            UpdateViewCard?.Invoke();
        }
    }
}