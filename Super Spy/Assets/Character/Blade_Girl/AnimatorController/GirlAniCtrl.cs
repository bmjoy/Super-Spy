using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class GirlAniCtrl : AniCtrl {
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			anim.SetBool ("run", ETCInput.GetAxis ("Vertical") != 0 ||
				ETCInput.GetAxis ("Horizontal") != 0);
			anim.SetBool ("pugong", !anim.GetBool("pugong") && ETCInput.GetButtonDown ("ButtonHit"));
			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit ();
			}
		}
	}
}
