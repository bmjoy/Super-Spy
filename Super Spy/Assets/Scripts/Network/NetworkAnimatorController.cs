using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class NetworkAnimatorController : NetworkBehaviour {
	protected Animator anim;

	public virtual void Start() {
		anim = GetComponent<Animator> ();
	}

	public void SetAnimation(string state, bool flag) {
		anim.SetBool (state, flag);
		CmdSetState (state, flag);
	}

	[ClientCallback]
	protected virtual void Update() {
		if (isLocalPlayer) {
			string state = "run";
			bool flag = ETCInput.GetAxis ("Vertical") != 0 ||
				ETCInput.GetAxis ("Horizontal") != 0;
			SetAnimation (state, flag);
		}
	}

	[Command]
	protected virtual void CmdSetState(string state, bool flag) {
		if (anim) {
			RpcSetState (state, flag);
		}
	}
		
	[ClientRpc]
	protected virtual void RpcSetState(string state, bool flag) {
		if (anim) {
			anim.SetBool (state, flag);
		}
	}


}