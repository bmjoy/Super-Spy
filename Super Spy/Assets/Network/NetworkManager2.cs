using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager2 : NetworkManager {
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		if (spawnPrefabs.Count > 0) {
			GameObject go = null;
			int i = Random.Range (0, spawnPrefabs.Count);
			go = spawnPrefabs [i];
			GameObject player;
			Transform startPos = GetStartPosition();
			if (startPos != null)
			{
				player = (GameObject)Instantiate(go, startPos.position, startPos.rotation);
			} else {
				player = (GameObject)Instantiate(go, Vector3.zero, Quaternion.identity);
			}

			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}
	}
}
