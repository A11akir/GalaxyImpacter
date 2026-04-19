using Feature.AI;
using Feature.Battlefield.Script;
using Feature.Battlefield.Script.View;
using Feature.Card.Script;
using Feature.Chakra;
using Feature.Common;
using Feature.Data;
using Feature.Entity.Script;
using Feature.GameSessionData;
using Feature.HandLogic;
using Feature.GameSessionFSM;
using Feature.Hero;
using Feature.ShopGamePlay.Script.Currency;
using Feature.ShopGamePlay.Script.ShopWindow;
using Feature.StagesGameLogic;
using Feature.Timer;
using Feature.UI;
using Feature.UI.SelectWindowHero;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Feature.EntryPoint.Script
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameData _gameData;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle().NonLazy();
            Container.Bind<CorrectableActivityGameObject>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HandCardsPositionSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SelectWindowHeroPresenter>().AsSingle();
            Container.Bind<SelectWindowHeroModel>().AsSingle();
            Container.Bind<SelectWindowHeroView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HandCardViews>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameSessionView>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<CurrencyManagePresenter>().AsSingle();
            Container.Bind<HandCardPresenter>().AsSingle();
            Container.Bind<CardOnBattlefieldPresenter>().AsSingle();
            Container.Bind<DeckFillSystem>().AsSingle();            
            Container.Bind<HandFillSystem>().AsSingle();       
            Container.Bind<TurnСycleGameSessionSystem>().AsSingle(); 
            Container.Bind<GameSessionPlayerData>().AsSingle();
            Container.Bind<GameSessionModel>().AsSingle();
            Container.Bind<GameSessionPresenter>().AsSingle();
            Container.Bind<AIRandomSelectSystem>().AsSingle();

            Container.Bind<CombatSystem.CombatSystem>().AsSingle();
            Container.Bind<GraphicRaycaster>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EventSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EntityDeathSystem>().AsSingle();
            Container.Bind<CreateOwnerCardAndHealthEntitySystem>().AsSingle();
            
            Container.Bind<HandViewSwitcher>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ChakraWindowPresenter>().AsSingle();
            Container.Bind<CurrencyManageView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ChakraWindowView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<CurrencyManagerSystem>().AsSingle();
            Container.Bind<ChakraManagerSystem>().AsSingle();
            Container.Bind<CardCastSystem>().AsSingle();
            
            Container.Bind<HandDataRepository>().AsSingle();            
            Container.Bind<GameplayLogicCard>().AsSingle();
            Container.Bind<TargetingSystem>().AsSingle();
            Container.Bind<CastCardAreaMinion>().FromComponentInHierarchy().AsSingle();

            Container.Bind<BattlefieldSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TipPlaceBattlefieldViewSystem>().FromComponentInHierarchy().AsSingle();
            
            
            Container.Bind<ShopGameplayManagerSystem>().AsSingle();            
            Container.Bind<ShopGameplayPresenter>().AsSingle();             
            Container.Bind<ShopGameplayView>().FromComponentInHierarchy().AsSingle();   
            Container.Bind<FightStatePresenter>().AsSingle(); 
            Container.Bind<FightStateView>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<PrepareStatePresenter>().AsSingle(); 
            Container.Bind<PrepareStateView>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<ReadyStageBackOrFightSystem>().AsSingle(); 
            
            Container.Bind<TimerSystem>().AsSingle();
            Container.Bind<TimerStageGameSessionSystem>().AsSingle();
            Container.Bind<TimerStageGameSessionPresenter>().AsSingle();
            Container.Bind<TimerStageGameSessionSystemView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UpdateSystem>().FromComponentInHierarchy().AsSingle();
            
            BindGameSessionFSM();
            BindConfig();
      
        }
        
        private void BindGameSessionFSM()
        {
            Container.Bind<GameSessionFSM.GameSessionFSM>().FromComponentInHierarchy().AsSingle();
            Container.Bind<StartStateGameSessionFSM>().AsTransient();
            Container.Bind<BanStateGameSessionFSM>().AsTransient();
            Container.Bind<PickStateGameSessionFSM>().AsTransient();
            Container.Bind<FightStateGameSessionFSM>().AsTransient();
            Container.Bind<PrepareStateGameSessionFSM>().AsTransient();
            Container.Bind<BlockStateGameSessionFSM>().AsTransient();
        }

        private void BindConfig()
        {
            Container.Bind<GameData>().FromInstance(_gameData).AsSingle().Lazy();
            Container.Bind<HeroStatsData>().AsSingle();
            Container.Bind<CardStatsData>().AsSingle();
        }
    }
}