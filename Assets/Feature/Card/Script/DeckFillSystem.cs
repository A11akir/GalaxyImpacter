using System.Collections.Generic;
using System.Linq;
using Feature.Data;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Card.Script
{
    public class DeckFillSystem
    {
        private GameData _gameData;

        public DeckFillSystem(GameData gameData) => _gameData = gameData;

        public void InitializeDeck(CardAndHealthEntityOwnerData hero)
        {
            hero.ClearDeck();

            List<CardStatsData> deck;
            
            if (hero.SpellsList != null && hero.SpellsList.Count > 0)
            {
                deck = BuildMinionDeck(hero);
            }
            else
            {
                deck = BuildHeroDeck(hero);
            }

            foreach (var card in deck)
            {
                var cardCopy = ScriptableObject.Instantiate(card);
                cardCopy.id = System.Guid.NewGuid().ToString();
                hero.AddCardToDeck(cardCopy);
            }

            hero.SetBaseDeck(hero.CardsInDeck.CurrentValue.ToList());
        }

        private List<CardStatsData> BuildHeroDeck(CardAndHealthEntityOwnerData hero)
        {
            var deck = new List<CardStatsData>();

            var classCards = _gameData.GetCardsByHeroName(hero._heroName);

            var classMinions = classCards.OfType<MinionCardData>()
                .Where(c => c.Rarity == CardRarity.Common)
                .ToList();

            var classSpells = classCards.OfType<SpellCardData>()
                .Where(c => c.Rarity == CardRarity.Common)
                .ToList();
            
            var baseMinions = _gameData.baseCards.OfType<MinionCardData>().ToList();
            var baseSpells = _gameData.baseCards.OfType<SpellCardData>().ToList();
            
            AddRandom(deck, baseMinions, 1);
            
            AddRandom(deck, baseSpells, 2);
            
            AddRandom(deck, classMinions, 1);
            
            AddRandom(deck, classSpells, 2);

            return deck;
        }

        private List<CardStatsData> BuildMinionDeck(CardAndHealthEntityOwnerData hero)
        {
            var source = new List<CardStatsData>(hero.SpellsList);
            
            for (int i = source.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (source[i], source[j]) = (source[j], source[i]);
            }

            int count = Mathf.Min(hero.startCardsInDeckCount, source.Count);
            return source.Take(count).ToList();
        }

        private void AddRandom<T>(List<CardStatsData> deck, List<T> source, int count)
            where T : CardStatsData
        {
            if (source.Count == 0) return;

            var shuffled = source.OrderBy(_ => Random.value).ToList();
            deck.AddRange(shuffled.Take(count));
        }
    }
}