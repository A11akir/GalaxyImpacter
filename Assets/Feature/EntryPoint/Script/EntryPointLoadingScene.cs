using Scenes.Script;
using Zenject;

namespace Feature.EntryPoint.Script
{
    public class EntryPointLoadingScene : IInitializable
    {
        private readonly ZenjectSceneLoader _sceneLoader;

        public EntryPointLoadingScene(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _sceneLoader.LoadScene(SceneName.GameplayScene.ToSceneString());
        }
    }
}