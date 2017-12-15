using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SelectPlayer : MonoBehaviour {
	public GameObject red_player;
	public GameObject blue_player;

	NetworkManager manager;

	float cur_time;

	void Awake() {
		manager = GetComponent<NetworkManager>();
	}

	void Start() {
		cur_time = 0;
	}

	void Update() {
		if (Time.time - cur_time > 3) {
			if (manager == null) {
				manager = GetComponent<NetworkManager>();
			}
			if (manager.playerPrefab.tag == "Red") {
				manager.playerPrefab = blue_player;
			} else {
				manager.playerPrefab = red_player;
			}
			cur_time = Time.time;
		}
	}
}
