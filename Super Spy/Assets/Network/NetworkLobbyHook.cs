using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
	public GameObject[] playerPrefabs;
	int cnt;
	void Start() {
		cnt = 0;
	}
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
		cnt = (cnt + 1) % playerPrefabs.Length;
		GetComponent<LobbyManager> ().gamePlayerPrefab = playerPrefabs [cnt];
    }
}
