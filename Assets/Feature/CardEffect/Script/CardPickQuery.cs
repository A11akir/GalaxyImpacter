using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Hero;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class CardPickQuery
    {
        public ClassSource ClassSource;
        public AllHeroClass ManualClass;
        public CardTypeFilter CardType;
        public RaritySource RaritySource;
        public List<CardRarity> ManualRarities;
    }
}