using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;


public class MultiplayerManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] GameObject playerPrefab;
    private NetworkRunner _runner;

    Dictionary<PlayerRef, NetworkObject> playerDict = new Dictionary<PlayerRef, NetworkObject>();

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        if (runner.IsServer) {
            Vector3 spawnPos = new Vector3(Random.Range(-5, 5), 1f, Random.Range(-5, 5));
            NetworkObject playerObj = runner.Spawn(playerPrefab, spawnPos, Quaternion.identity, player);
            playerDict.Add(player, playerObj);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {

        if (runner.IsServer)
        {
            if (playerDict.TryGetValue(player, out NetworkObject playerObj)) {
                playerDict.Remove(player);
                runner.Despawn(playerObj);

            }
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {
    
        NetworkInputData _inputData = new NetworkInputData();
        _inputData.direction.x = Input.GetAxis("Horizontal");
        _inputData.direction.z = Input.GetAxis("Vertical");
        _inputData.buttons.Set(MyInputButton.Jump, Input.GetButton("Jump"));
        _inputData.buttons.Set(MyInputButton.Teleport, Input.GetButtonDown("Teleport"));
        input.Set(_inputData);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

}
