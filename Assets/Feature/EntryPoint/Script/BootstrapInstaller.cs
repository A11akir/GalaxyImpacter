using Feature.Common;
using Feature.Data;
using Feature.GameSessionData;
using Feature.HandLogic;
using Feature.GameSessionFSM;
using Feature.UI;
using Feature.UI.SelectWindowHero;
using UnityEngine;
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
            Container.Bind<HeroView>().FromComponentInHierarchy().AsTransient(); //Check Trouble
            
            Container.Bind<GameSessionPlayerData>().AsSingle();
            Container.Bind<GameSessionData.GameSessionData>().AsTransient();

            
            
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
            Container.Bind<GameData>().FromInstance(_gameData).AsSingle().NonLazy();
        }
    }
}