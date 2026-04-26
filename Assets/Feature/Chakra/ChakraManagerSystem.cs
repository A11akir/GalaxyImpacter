using Feature.Chakra;
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

        public void Init(CardAndHealthEntityOwnerData owner, ChakraWindowView chakraWindowView)
        {
            _chakraWindowPresenter.SubscribeToChakraChanges(owner, chakraWindowView);
        }

        public void InitEntityChakra(CardAndHealthEntityOwnerData owner)
        {
            owner.SetChakra(owner.StartChakra);
        }

        public void NewTurnUpdate()
        {
            var playerHero = _gameSessionModel.PlayerHero.MainHeroEntity();
            var enemyHero = _gameSessionModel.EnemyHero.MainHeroEntity();

            IncrementHeroChakra(playerHero);
            IncrementHeroChakra(enemyHero);

            foreach (var owner in _gameSessionModel.GetAllEntityOwners())
            {
                if (owner != playerHero && owner != enemyHero)
                    RefillMinionChakra(owner);
            }
        }
        
        private void IncrementHeroChakra(CardAndHealthEntityOwnerData hero)
        {
            int increment = 2;
            int max = hero.MaxChakraCountBaseIncrease;
            int newChakra = Mathf.Min(_gameSessionModel.Turn + increment, max);
            hero.SetChakra(newChakra);
        }
        private void RefillMinionChakra(CardAndHealthEntityOwnerData minion)
        {
            minion.SetChakra(minion.StartChakra);
        }

        private void AddChakraWithMaxLimit(CardAndHealthEntityOwnerData owner, int amount)
        {
            int max = owner.MaxChakraCountBaseIncrease;
            int newChakra = Mathf.Min(owner.Chakra + amount, max);
            owner.SetChakra(newChakra);
        }
    }
}