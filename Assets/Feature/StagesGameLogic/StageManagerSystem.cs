using Feature.AI;
using Feature.Card.Script;
using Feature.ShopGamePlay.Script.ShopWindow;
using Feature.Timer;

namespace Feature.StagesGameLogic
{
    public class StageManagerSystem
    {
        private readonly TargetingSystem _targetingSystem;
        private readonly PrepareStatePresenter _prepareStatePresenter;
        private readonly FightStatePresenter _fightStatePresenter;
        private readonly AISystem _aiSystem;
        private readonly ShopGameplayManagerSystem _shopSystem;
        private readonly TimerStageGameSessionSystem _timerSystem;
    
        public bool IsPreparePhase { get; private set; }

        public StageManagerSystem(TargetingSystem targetingSystem, PrepareStatePresenter prepareStatePresenter, FightStatePresenter fightStatePresenter, AISystem aiSystem, ShopGameplayManagerSystem shopSystem, TimerStageGameSessionSystem timerSystem /* 6 зависимостей вместо 17 */)
        {
            _targetingSystem = targetingSystem;
            _prepareStatePresenter = prepareStatePresenter;
            _fightStatePresenter = fightStatePresenter;
            _aiSystem = aiSystem;
            _shopSystem = shopSystem;
            _timerSystem = timerSystem;
        }

        public void StartPreparePhase(int turn)
        {
            IsPreparePhase = true;
            _targetingSystem.IsPreparePhase = true;
        
            _prepareStatePresenter.StartPrepare();
            _shopSystem.UnlockShop();
            _timerSystem.StartTimerPrepare(turn);
            _aiSystem.ExecutePreparePhase();
        }

        public void StartFightPhase(int turn)
        {
            IsPreparePhase = false;
            _targetingSystem.IsPreparePhase = false;
        
            _fightStatePresenter.StartFight();
            _shopSystem.LockShop();
            _timerSystem.StartTimerFight(turn);
            _aiSystem.ExecuteFightPhase();
        }

        public void EndPreparePhase() => _prepareStatePresenter.EndPrepare();
        public void EndFightPhase() => _fightStatePresenter.EndFight();
    }
}