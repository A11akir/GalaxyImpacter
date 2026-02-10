using Feature.Steam;
using Zenject;

namespace Feature.EntryPoint.Script
{
    public class LoadingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntryPointLoadingScene>().AsSingle();
            Container.Bind<SteamStart>().FromComponentInHierarchy().AsSingle();
        }
    }
}