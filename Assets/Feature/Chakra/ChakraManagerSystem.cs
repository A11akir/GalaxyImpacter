using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Chakra
{
    public class ChakraManagerSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly ChakraWindowPresenter _chakraWindowPresenter;

        public ChakraManagerSystem(GameSessionModel gameSessionModel, ChakraWindowPresenter chakraWindowPresenter)
        {
            _gameSessionModel = gameSessionModel;
            _chakraWindowPresenter = chakraWindowPresenter;
        }

        public void Init()
        {
            _chakraWindowPresenter.SubscribeToChakraChanges();
        }
        
        public void NewTurnUpdate()
        {
            AddChakraHeroForNewTurn();
        }

        private void AddChakraHeroForNewTurn()
        {
            AddChakraWithMaxLimit(_gameSessionModel.EnemyHero, 2);
            AddChakraWithMaxLimit(_gameSessionModel.PlayerHero, 2);
        }

        private void AddChakraWithMaxLimit(GameSessionPlayerData hero, int amount)
        {
            int maxChakra = hero.MaxChakraCountBaseIncrease;
            int newChakra = Mathf.Min(hero.Chakra + amount, maxChakra);
            hero.Chakra = newChakra;
        }
    }
}