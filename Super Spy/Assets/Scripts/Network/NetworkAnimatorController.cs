using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkAnimatorController : NetworkBehaviour {
	protected Animator anim;

	public virtual void Start() {
		anim = GetComponent<Animator> ();
	}

	public void SetAnimation(string state) {
		anim.SetTrigger (state);
	}
		
	protected virtual void Update() {
		if (isLocalPlayer) {
			anim.SetBool ("run", ETCInput.GetAxis ("Vertical") != 0 ||
			ETCInput.GetAxis ("Horizontal") != 0);
		}
	}
}