using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.GameSessionData;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Feature.Battlefield.Script
{
    public class BattlefieldSystem : MonoBehaviour
    {
        private CardOnBattlefieldPresenter _cardOnBattlefieldPresenter;
        private BattlefieldCardTransformSystem _battlefieldCardTransformSystem;
        private GameSessionModel _gameSessionModel;
        
        [SerializeField] private GameObject _enemyBattlefield;
        [SerializeField] private GameObject _playerBattlefield;
        
        [ShowInInspector] private List<CardOnBattlefieldView> _cardsGameObjectsEnemy;        
        [ShowInInspector] private List<CardOnBattlefieldView> _cardsGameObjectsPlayer;

        [Inject]
        public void Construct(GameSessionModel gameSessionModel, BattlefieldCardTransformSystem battlefieldCardTransformSystem, CardOnBattlefieldPresenter cardOnBattlefieldPresenter)
        {
            _gameSessionModel = gameSessionModel;
            _battlefieldCardTransformSystem = battlefieldCardTransformSystem;
            _cardOnBattlefieldPresenter = cardOnBattlefieldPresenter;
        }

        public void Init()
        {
            _gameSessionModel.PlayerHero.CardsInBoard
                .Subscribe(_ =>
                {
                    UpdateCardToBattlefield();
                });
            
            _cardsGameObjectsPlayer = _playerBattlefield
                .GetComponentsInChildren<CardOnBattlefieldView>(true)
                .ToList();            
            _cardsGameObjectsEnemy = _enemyBattlefield
                .GetComponentsInChildren<CardOnBattlefieldView>(true)
                .ToList();
        }

        private void UpdateCardToBattlefield()
        {
            Debug.Log("UpdateCardToBattlefield");
            for (int i = 0; i < _gameSessionModel.PlayerHero.CardsInBoard.CurrentValue.Count; i++)
            {
                Debug.Log(_gameSessionModel.PlayerHero.CardsInBoard.CurrentValue.Count.ToString());
                _cardOnBattlefieldPresenter.SetCardInPlayerHand(_cardsGameObjectsPlayer[i], _gameSessionModel.PlayerHero.CardsInBoard.CurrentValue[i]);
            }
            _battlefieldCardTransformSystem.UpdateCardsPosition();
        }

        public void AddCardInBattlefield(GameSessionPlayerData playerData, CardStatsData cardData)
        {
            if (_gameSessionModel.PlayerHero == playerData)
            {
                playerData.AddCardToBoard(cardData);
            }
        }
    }
}