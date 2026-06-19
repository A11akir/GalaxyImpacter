// 2. BattlefieldSystem — с EntityOwnerFactory внутри

using System.Linq;
using Feature.Battlefield.Script.View;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.HandLogic;
using Feature.Hero;
using UnityEngine;
using Zenject;

namespace Feature.Battlefield.Script
{
    public class BattlefieldSystem : MonoBehaviour
    {
        [SerializeField] private GameObject enemyBattlefield;
        [SerializeField] private GameObject playerBattlefield;

        private GameSessionModel _gameSessionModel;
        private BoardManager _boardManager;
        private BattlefieldViewManager _viewManager;
        private CreateOwnerCardAndHealthEntitySystem _createOwnerSystem;
        private TargetingSystem _targetingSystem;
        private HandViewSwitcher _handViewSwitcher;

        [Inject]
        public void Construct(
            GameSessionModel gameSessionModel,
            BoardManager boardManager,
            BattlefieldViewManager viewManager,
            CreateOwnerCardAndHealthEntitySystem createOwnerSystem,
            TargetingSystem targetingSystem,
            HandViewSwitcher handViewSwitcher)
        {
            _gameSessionModel = gameSessionModel;
            _boardManager = boardManager;
            _viewManager = viewManager;
            _createOwnerSystem = createOwnerSystem;
            _targetingSystem = targetingSystem;
            _handViewSwitcher = handViewSwitcher;

            _boardManager.OnCardAdded += OnCardAdded;
            _boardManager.OnCardRemoved += OnCardRemoved;
            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        public void Init()
        {
            _viewManager.InitializeViews(_gameSessionModel.PlayerHero, playerBattlefield);
            _viewManager.InitializeViews(_gameSessionModel.EnemyHero, enemyBattlefield);

            _boardManager.Subscribe(_gameSessionModel.PlayerHero);
            _boardManager.Subscribe(_gameSessionModel.EnemyHero);
        }

        public void AddCardInBattlefield(GameSessionPlayerData playerData, CardStatsData cardData)
        {
            bool isEnemy = playerData == _gameSessionModel.EnemyHero;
        
            int index = isEnemy 
                ? _boardManager.GetRandomFreeSlotForEnemy()
                : _boardManager.GetFreeSlotForPlayer();

            if (index == -1) return;

            playerData.AddCardToBoard((MinionCardData)cardData, index);
        }

        private void OnCardAdded(MinionCardData card, int index, GameSessionPlayerData playerData)
        {
            var view = _viewManager.SetupView(card, index, playerData);
            var owner = CreateOwnerFromCard(card); // ← factory внутри
            
        
            playerData.CardAndHealthEntityOwners.Add(owner);
            _viewManager.RegisterOwnerView(owner, view);
            _targetingSystem.RegisterTarget(view.gameObject, owner);

            bool isEnemy = playerData == _gameSessionModel.EnemyHero;
        
            if (!isEnemy)
            {
                _boardManager.OccupySlot(index);
                view.OnClicked += () => _handViewSwitcher.SwitchTo(owner);
            }

            if (isEnemy)
                _createOwnerSystem.CreateEntityEnemy(owner, view);
            else
                _createOwnerSystem.CreateEntityPlayer(owner, view);
        }

        private void OnCardRemoved(MinionCardData card, GameSessionPlayerData playerData)
        {
            var owner = playerData.CardAndHealthEntityOwners
                .FirstOrDefault(o => o.CardId == card.id);
        
            if (owner == null) return;

            var view = _viewManager.GetView(owner);
            int slot = _viewManager.GetViewIndex(view, playerData);
        
            if (slot >= 0)
                _boardManager.FreeSlot(slot);

            if (_handViewSwitcher.CurrentOwner == owner)
                _handViewSwitcher.SwitchTo(playerData.MainHeroEntity());

            _viewManager.UnregisterOwnerView(owner);
            playerData.CardAndHealthEntityOwners.Remove(owner);
        }

        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            _viewManager.SetSelected(owner, true);
        }
        
        private CardAndHealthEntityOwnerData CreateOwnerFromCard(MinionCardData card)
        {
            return new CardAndHealthEntityOwnerData
            {
                CardId = card.id,
                startCardsInDeckCount = card.SpellsList.Count,
                startCardsInHandToDraw = card.HandCardCount,
                _heroName = card.Name,
                HealthValue = card.Health,
                MaxHealth = card.Health,
                Chakra = card.Chakra,
                _iconImage = card.IconImage,
                SpellsList = card.SpellsList,
                StartChakra = card.Chakra,
                Cost = card.Cost,
            };
        }
    }
}