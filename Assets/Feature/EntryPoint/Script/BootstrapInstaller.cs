using Feature.Common;
using Feature.HandLogic;
using Feature.GameSessionFSM;
using Zenject;

namespace Feature.EntryPoint.Script
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle().NonLazy();
            Container.Bind<CorrectableActivityGameObject>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HandCardsPositionSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameSessionFSM.GameSessionFSM>().AsSingle();
        }
    }
}