using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RedBlue : MonoBehaviour {
	int red;
	int blue;

	public GameObject red_prefab;
	public GameObject blue_prefab;

	void Start() {
		red = blue = 5;
	}
	public void PlayerStop(string player_tag) {
		if (player_tag == "Red") {
			++red;
		} else {
			++blue;
		}
	}
	public void ChangePrefab() {
		int i = Random.Range (0, 2);
		if (i == 1) {
			if (red > 0) {
				red--;
				GetComponent<NetworkManager> ().playerPrefab = red_prefab;
			} else {
				if (blue > 0) {
					blue--;
					GetComponent<NetworkManager> ().playerPrefab = blue_prefab;
				} else {
					GetComponent<NetworkManager> ().playerPrefab = null;
				}
			}
		} else {
			if (blue > 0) {
				blue--;
				GetComponent<NetworkManager> ().playerPrefab = blue_prefab;
			} else {
				if (red > 0) {
					red--;
					GetComponent<NetworkManager> ().playerPrefab = red_prefab;
				} else {
					GetComponent<NetworkManager> ().playerPrefab = null;
				}
			}
		}
	}
}
