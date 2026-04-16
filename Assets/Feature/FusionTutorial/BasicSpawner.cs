using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feature.FusionTutorial
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 Direction;
    }
    
    public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _runner;

        [SerializeField] private NetworkPrefabRef _playerPrefab;
        [SerializeField] private Transform _playerParent;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();
        
        async void StartGame(Fusion.GameMode mode)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();

            if (scene.IsValid)
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
    
        private void OnGUI()
        {
            if (_runner == null)
            {
                if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
                    StartGame(Fusion.GameMode.Host);

                if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
                    StartGame(Fusion.GameMode.Client);
            }
        }
    

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }



        void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                Vector3 pos = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
                var obj = runner.Spawn(_playerPrefab, pos, Quaternion.identity, player);
                obj.transform.SetParent(_playerParent);
                obj.transform.localPosition = Vector3.zero;
                _spawnedCharacters.Add(player, obj);
            }
        }

        void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out var obj))
            {
                runner.Despawn(obj);
                _spawnedCharacters.Remove(player);
            }
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            if (Input.GetKey(KeyCode.W)) data.Direction += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) data.Direction += Vector3.back;
            if (Input.GetKey(KeyCode.A)) data.Direction += Vector3.left;
            if (Input.GetKey(KeyCode.D)) data.Direction += Vector3.right;

            input.Set(data);
        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}
