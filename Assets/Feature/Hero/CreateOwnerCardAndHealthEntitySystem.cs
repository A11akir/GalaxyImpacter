using System.Collections.Generic;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.Chakra;
using Feature.Entity.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.HandLogic;
using Feature.Health;
using Feature.UI;
using UnityEngine;

namespace Feature.Hero
{
    public class CreateOwnerCardAndHealthEntitySystem
    {
        private readonly HandViewSwitcher _handViewSwitcher;
        private readonly GameSessionModel _gameSessionModel;
        private readonly DeckFillSystem _deckFillSystem;
        private readonly HandDataRepository _handDataRepository;
        private readonly EntityDeathSystem _entityDeathSystem;
        private readonly ChakraManagerSystem _chakraManagerSystem;
        private readonly HandFillSystem _handFillSystem;
        private readonly HeroPowerSystem _heroPowerSystem;
        private readonly HeroPowerPresenter _heroPowerPresenter;
        private readonly CombatSystem.CombatSystem _combatSystem;

        private readonly Dictionary<CardAndHealthEntityOwnerData, EntityPresenter> _entityPresenters = new();

        public CreateOwnerCardAndHealthEntitySystem(GameSessionModel gameSessionModel,
            ChakraManagerSystem chakraManagerSystem, HandDataRepository handDataRepository,
            DeckFillSystem deckFillSystem, HandViewSwitcher handViewSwitcher, HandFillSystem handFillSystem,
            EntityDeathSystem entityDeathSystem, HeroPowerSystem heroPowerSystem, HeroPowerPresenter heroPowerPresenter,
            CombatSystem.CombatSystem combatSystem)
        {
            _gameSessionModel = gameSessionModel;
            _chakraManagerSystem = chakraManagerSystem;
            _handDataRepository = handDataRepository;
            _deckFillSystem = deckFillSystem;
            _handViewSwitcher = handViewSwitcher;
            _handFillSystem = handFillSystem;
            _entityDeathSystem = entityDeathSystem;
            _heroPowerSystem = heroPowerSystem;
            _heroPowerPresenter = heroPowerPresenter;
            _combatSystem = combatSystem;
            _entityDeathSystem.OnEntityDied += DisposeEntity;
        }

        public void CreatePlayersEntity(
            IHealthView playerHealthView,
            IHealthView enemyHealthView,
            List<HeroPowerGameplayView> playerHeroPowerViews, // ← список
            List<HeroPowerGameplayView> enemyHeroPowerViews, // ← список
            List<SpellCardData> heroPowers, // ← список
            HandCardViews enemyHandCardViews)
        {
            CreateMainEnemyPlayer(playerHealthView, playerHeroPowerViews, enemyHeroPowerViews, heroPowers);
            CreateMainEnemyEntity(_gameSessionModel.EnemyHero.MainHeroEntity(), enemyHealthView, enemyHandCardViews);
        }

        private void CreateMainEnemyPlayer(
            IHealthView playerHealthView,
            List<HeroPowerGameplayView> playerHeroPowerViews,
            List<HeroPowerGameplayView> enemyHeroPowerViews,
            List<SpellCardData> heroPowers)
        {
            var playerEntity = _gameSessionModel.PlayerHero.MainHeroEntity();
            CreateEntityPlayer(playerEntity, playerHealthView);
            _handViewSwitcher.SwitchTo(playerEntity);

            _gameSessionModel.PlayerHero.HeroPowerUsage.Init(heroPowers.Count);

            for (int i = 0; i < heroPowers.Count && i < playerHeroPowerViews.Count; i++)
            {
                _heroPowerSystem.Init(
                    playerEntity,
                    playerHeroPowerViews[i].gameObject,
                    heroPowers[i],
                    _gameSessionModel.PlayerHero,
                    i);
            }

            ApplyHeroPowerPassives(playerEntity, heroPowers);

            _heroPowerPresenter.InitPlayer(playerHeroPowerViews, heroPowers.Count);
            _heroPowerPresenter.InitEnemy(enemyHeroPowerViews);
        }

        private void CreateMainEnemyEntity(CardAndHealthEntityOwnerData owner, IHealthView healthView,
            HandCardViews handCardViews)
        {
            _deckFillSystem.InitializeDeck(owner);
            _handDataRepository.InitHandRepository(owner, handCardViews, isHidden: true);
            _gameSessionModel.EnemyHero.InitBoard();
            InitEntityCore(owner, healthView);
        }

        public void CreateEntityPlayer(CardAndHealthEntityOwnerData owner, IHealthView healthView)
        {
            _deckFillSystem.InitializeDeck(owner);
            var container = _handViewSwitcher.RegisterOwner(owner);
            _handDataRepository.InitHandRepository(owner, container.HandCardViews);
            _chakraManagerSystem.Init(owner, container.ChakraWindowView);
            InitEntityCore(owner, healthView);
        }

        public void CreateEntityEnemy(CardAndHealthEntityOwnerData owner, IHealthView healthView)
        {
            _deckFillSystem.InitializeDeck(owner);
            InitEntityCore(owner, healthView);
        }

        private void InitEntityCore(CardAndHealthEntityOwnerData owner, IHealthView healthView)
        {
            _handFillSystem.FillEntityHand(owner);
            _chakraManagerSystem.InitEntityChakra(owner);
            _entityDeathSystem.Init(owner);
            _entityPresenters[owner] = new EntityPresenter(owner, healthView);
        }

        private void ApplyHeroPowerPassives(CardAndHealthEntityOwnerData owner, List<SpellCardData> heroPowers)
        {
            foreach (var heroPower in heroPowers)
            {
                foreach (var effect in heroPower.Effects)
                {
                    if (effect is AddPassiveEffect addPassive)
                        addPassive.Execute(new EffectContext
                        {
                            Caster = owner,
                            CombatSystem = _combatSystem,
                            GameSessionModel = _gameSessionModel,
                            CardData = heroPower
                        });
                }
            }
        }

        private void DisposeEntity(CardAndHealthEntityOwnerData victim, CardAndHealthEntityOwnerData killer)
        {
            if (_entityPresenters.TryGetValue(victim, out var presenter))
            {
                presenter.Dispose();
                _entityPresenters.Remove(victim);
            }
        }
    }
}