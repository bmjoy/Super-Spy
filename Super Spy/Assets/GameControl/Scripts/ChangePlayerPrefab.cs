using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChangePlayerPrefab : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			CmdChangePlayer ();
		}
	}

	[Command]
	void CmdChangePlayer() {
		GameObject.Find ("NetworkManager");
	}
}
