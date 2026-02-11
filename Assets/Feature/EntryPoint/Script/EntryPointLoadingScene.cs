using Feature.Steam;
using Scenes.Script;
using Zenject;

namespace Feature.EntryPoint.Script
{
    public class EntryPointLoadingScene : IInitializable
    {
        private readonly ZenjectSceneLoader _sceneLoader;
        private readonly SteamStart _steamStart;

        public EntryPointLoadingScene(ZenjectSceneLoader sceneLoader, SteamStart steamStart)
        {
            _sceneLoader = sceneLoader;
            _steamStart = steamStart;
        }

        public void Initialize()
        {
           _steamStart.InitSteam();
            _sceneLoader.LoadScene(SceneName.GameplayScene.ToSceneString());
        }
    }
}