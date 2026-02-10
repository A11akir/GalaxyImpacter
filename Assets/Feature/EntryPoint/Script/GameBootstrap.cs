using Feature.Common;
using Feature.GameMode;
using UnityEngine;
using Zenject;

namespace Feature.EntryPoint.Script
{
    public class GameBootstrap : IInitializable
    {
        private readonly CorrectableActivityGameObject _correctableActivityGameObject;
        public GameBootstrap(CorrectableActivityGameObject correctableActivityGameObject)
        {
            _correctableActivityGameObject = correctableActivityGameObject;
        }
        
        public void CheckStartLevel()
        {
            if (GameModeSession.GameMode == GameMode.GameMode.Offline)
            {
                
            }
            else if (GameModeSession.GameMode == GameMode.GameMode.Online)
            {
                
            }
        }

        public void Initialize()
        {
            Debug.Log("GameBootstrap.Initialize()");
            _correctableActivityGameObject.SetCorrectableActivityGameObject();
        }
    }
}
