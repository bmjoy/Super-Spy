using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WizardAniCtrl : AniCtrl {
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			if (ETCInput.GetAxis ("Vertical") != 0 ||
			    ETCInput.GetAxis ("Horizontal") != 0) {
				anim.SetBool ("run", true);

			} else {
				anim.SetBool ("run", false);
			}

			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit ();
			}
		}
	}
}
