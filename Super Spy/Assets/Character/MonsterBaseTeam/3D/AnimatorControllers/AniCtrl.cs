using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class AniCtrl : NetworkBehaviour {
	public Vector3 origin_position;
	protected Animator anim;

	public virtual void Start() {
		if (isLocalPlayer) {
			transform.position = origin_position;
			anim = GetComponent<Animator> ();

			var controller = GameObject.FindWithTag ("GameController");
			if (controller) {
				controller.GetComponent<InitTarget> ().SetTarget (transform);
			}
		}
	}
}