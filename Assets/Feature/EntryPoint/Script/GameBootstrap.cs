using Feature.Common;
using Feature.GameMode;
using UnityEngine;
using Zenject;

namespace Feature.EntryPoint.Script
{
    public class GameBootstrap : IInitializable
    {
        private readonly CorrectableActivityGameObject _correctableActivityGameObject;
        private readonly GameSessionFSM.GameSessionFSM _gameSessionFSM;
        public GameBootstrap(CorrectableActivityGameObject correctableActivityGameObject, GameSessionFSM.GameSessionFSM gameSessionFsm)
        {
            _correctableActivityGameObject = correctableActivityGameObject;
            _gameSessionFSM = gameSessionFsm;
        }
        
        public void CheckStartLevel()
        {
            if (GameModeSession.GameMode == GameMode.GameMode.Offline)
            {
                Debug.Log("Starting Game Mode Offline");
                _gameSessionFSM.Initialize();
            }
            else if (GameModeSession.GameMode == GameMode.GameMode.Online)
            {
                Debug.Log("Starting Game Mode Online");
            }
        }

        public void Initialize()
        {
            Debug.Log("GameBootstrap.Initialize()");
            _correctableActivityGameObject.SetCorrectableActivityGameObject();
        }
    }
}
