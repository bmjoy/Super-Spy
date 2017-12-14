using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSkillController : NetworkBehaviour {
	GameObject effect;
	[Command]
	void CmdShowEffect() {
		GameObject.Instantiate (effect);
		RpcShowEffect ();
	}

	[ClientRpc]
	void RpcShowEffect() {
		GameObject.Instantiate (effect);
	}

	[ClientCallback]
	public void SyncEffect(GameObject e) {
		effect = e;
		CmdShowEffect ();
	}
}
