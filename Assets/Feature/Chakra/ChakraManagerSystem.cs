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
            foreach (var owner in _gameSessionModel.GetAllEntityOwners())
            {
                AddChakraWithMaxLimit(owner, owner.StartChakra + _gameSessionModel.Turn);
            }
        }

        private void AddChakraWithMaxLimit(CardAndHealthEntityOwnerData owner, int amount)
        {
            int max = owner.MaxChakraCountBaseIncrease;
            int newChakra = Mathf.Min(owner.Chakra + amount, max);
            owner.SetChakra(newChakra);
        }
    }
}