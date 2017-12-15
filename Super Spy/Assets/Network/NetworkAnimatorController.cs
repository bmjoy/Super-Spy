using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class NetworkAnimatorController : NetworkBehaviour {
	public Vector3 origin_position;
	protected Animator anim;

	public virtual void Start() {
		anim = GetComponent<Animator> ();
		transform.position = origin_position;
		if (isLocalPlayer) {
			var controller = GameObject.Find ("Canvas");
			controller.transform.Find ("SkillButtons").GetComponent<InitTarget> ().SetTarget (transform);
			var joystick = controller.GetComponentInChildren<ETCJoystick> ();
			joystick.axisX.directTransform = joystick.axisY.directTransform = joystick.cameraLookAt = transform;
			var button = controller.GetComponentInChildren<ETCButton> ();
			button.axis.directTransform = transform;
			if (tag == "Blue") {
				Camera.main.transform.Rotate (0, 0, 180);
			}
		}
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
			state = "pugong";
			flag = ETCInput.GetButtonDown ("ButtonHit");
			SetAnimation (state, flag);
			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit ();
			}
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