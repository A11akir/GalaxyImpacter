using Feature.Battlefield.Script;
using Feature.CardEffect.Script;
using Feature.ClassBranchWindow.Script;
using Feature.GameSessionData;
using Feature.Hero;
using Feature.Items.Scripts;
using Feature.PassiveEffect.Script;
using Feature.ShopGamePlay.Script.Currency;
using Feature.Timer;
using Feature.UI;
using UnityEngine;

namespace Feature.StagesGameLogic
{
    public class TurnCycleGameSessionSystem
    {
        private readonly StageManagerSystem _stageManagerSystem;
        private readonly TurnResourceManager _resourceManager;
        private readonly ReadyStageBackOrFightSystem _readySystem;
        private readonly GameSessionModel _gameSessionModel;
        private readonly GameSessionPresenter _gameSessionPresenter;
        private readonly BattlefieldSystem _battlefieldSystem;
        private readonly CurrencyManagerSystem _currencyManager;
        private readonly TimerStageGameSessionSystem _timerSystem;
        private readonly InventoryPresenter _inventoryPresenter;
        private readonly HeroClassLevelSystem _heroClassLevelSystem;
        private readonly ClassLevelWindowPresenter _classLevelWindowPresenter;
        private readonly TurnTriggerEffectQueue _turnEffectQueue;
        private readonly HandCostModifierSystem  _handCostModifierSystem;


        public TurnCycleGameSessionSystem(StageManagerSystem stageManagerSystem, TurnResourceManager resourceManager, GameSessionModel gameSessionModel, 
            GameSessionPresenter gameSessionPresenter, BattlefieldSystem battlefieldSystem, CurrencyManagerSystem currencyManager,
            TimerStageGameSessionSystem timerSystem, ReadyStageBackOrFightSystem readySystem, InventoryPresenter inventoryPresenter, HeroClassLevelSystem heroClassLevelSystem, ClassLevelWindowPresenter classLevelWindowPresenter, TurnTriggerEffectQueue turnEffectQueue, HandCostModifierSystem handCostModifierSystem)
        {
            _stageManagerSystem = stageManagerSystem;
            _resourceManager = resourceManager;
            _gameSessionModel = gameSessionModel;
            _gameSessionPresenter = gameSessionPresenter;
            _battlefieldSystem = battlefieldSystem;
            _currencyManager = currencyManager;
            _timerSystem = timerSystem;
            _readySystem = readySystem;
            _inventoryPresenter = inventoryPresenter;
            _heroClassLevelSystem = heroClassLevelSystem;
            _classLevelWindowPresenter = classLevelWindowPresenter;
            _turnEffectQueue = turnEffectQueue;
            _handCostModifierSystem = handCostModifierSystem;
        }


        public void StartGameSession()
        {
            _timerSystem.ShowTimer();
            _gameSessionPresenter.SetupEntityViews();
            _gameSessionModel.PlayerHero.InitBoard();
            _battlefieldSystem.Init();
            _currencyManager.Init();
            _inventoryPresenter.Init();
            _heroClassLevelSystem.Init();
            _classLevelWindowPresenter.Init();
        
            _stageManagerSystem.StartPreparePhase(_gameSessionModel.Turn);
        }

        public void CycleStartPrepareTurn()
        {
            _readySystem.Reset();
            _resourceManager.StartNewTurn();

            TriggerTurnStartEffects(); // ← начало фазы подготовки

            _stageManagerSystem.StartPreparePhase(_gameSessionModel.Turn);
        }

        public void CycleStartFightTurn()
        {
            _readySystem.Reset();
            _stageManagerSystem.StartFightPhase(_gameSessionModel.Turn);
        }

        public void CycleEndFightTurn()
        {
            TriggerTurnEndEffects();
            _handCostModifierSystem.ClearAllModifiers();
            _stageManagerSystem.EndFightPhase();
        }

        public void CycleEndPrepareTurn() => _stageManagerSystem.EndPreparePhase();
        
        private void TriggerTurnStartEffects()
        {
            foreach (var owner in _gameSessionModel.GetAllEntityOwners())
                owner.PassiveEffects.EnqueuePermanentTurnEffects(_turnEffectQueue, TurnTriggerTiming.TurnStart);

            _turnEffectQueue.TriggerAll(TurnTriggerTiming.TurnStart);
        }

        private void TriggerTurnEndEffects()
        {
            foreach (var owner in _gameSessionModel.GetAllEntityOwners())
                owner.PassiveEffects.EnqueuePermanentTurnEffects(_turnEffectQueue, TurnTriggerTiming.TurnEnd);

            _turnEffectQueue.TriggerAll(TurnTriggerTiming.TurnEnd);

            foreach (var owner in _gameSessionModel.GetAllEntityOwners())
                owner.PassiveEffects.CleanupExpiredPassives();
        }
    }
}