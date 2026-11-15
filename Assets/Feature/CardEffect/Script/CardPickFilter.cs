using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Hero;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class CardPickFilter
    {
        public ClassSource ClassSource;
        public AllHeroClass ManualClass;
        public CardTypeFilter CardType;
        public RaritySource RaritySource;
        public List<CardRarity> ManualRarities;

        /// <summary>
        /// Конкретные карты: одна — точечная генерация, несколько — случайная из списка.
        /// Пусто — пул по классу/редкости.
        /// </summary>
        [SerializeField] public CardStatsData[] SpecificCard = Array.Empty<CardStatsData>();
    }
}