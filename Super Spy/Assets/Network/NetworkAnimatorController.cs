using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class NetworkAnimatorController : NetworkBehaviour {
	public Vector3 origin_position;
	public float respam_time;
	float _time;
	protected Animator anim;

	public virtual void Start() {
		anim = GetComponent<Animator> ();
		transform.position = origin_position;
		_time = respam_time;

		if (isLocalPlayer) {
			var canvas = GameObject.Find ("Canvas");
			canvas.transform.Find ("SkillButtons").GetComponent<InitTarget> ().SetTarget (transform);
			var joystick = canvas.GetComponentInChildren<ETCJoystick> ();
			if (gameObject.tag == "Blue") {
				canvas.transform.Find ("Minimap/MiniMapBg/Mask/Bg").rotation = Quaternion.Euler (0, 0, 180);
				joystick.TurnAndMove = -1;
				joystick.followOffset.z *= -1;
				canvas.GetComponentInChildren<MiniMapCameraManager> ().flag = -1;
			}
			joystick.axisX.directTransform = joystick.axisY.directTransform = joystick.cameraLookAt = transform;
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