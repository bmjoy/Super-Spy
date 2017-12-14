using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class NetworkAnimatorController : NetworkBehaviour {
	public Vector3 origin_position;
	public GameObject[] skills;
	public float[] life_time;
	protected Animator anim;
	protected ETCJoystick joystick;

	public virtual void Start() {
		anim = GetComponent<Animator> ();
		transform.position = origin_position;

		if (isLocalPlayer) {
			var controller = GameObject.Find ("Canvas");
			controller.transform.Find ("SkillButtons").GetComponent<InitTarget> ().SetTarget (transform);
			joystick = controller.GetComponentInChildren<ETCJoystick> ();
			joystick.axisX.directTransform = joystick.axisY.directTransform = joystick.cameraLookAt = transform;
			var button = controller.GetComponentInChildren<ETCButton> ();
			button.axis.directTransform = transform;
		}
	}

	public void SetAnimation(string state, bool flag, Vector3 pos) {
		anim.SetBool (state, flag);
		GenerateEffect (state, flag, pos);
		CmdSetState (state, flag, pos);
	}

	[ClientCallback]
	protected virtual void Update() {
		if (isLocalPlayer) {
			string state = "run";
			bool flag = ETCInput.GetAxis ("Vertical") != 0 ||
				ETCInput.GetAxis ("Horizontal") != 0;
			SetAnimation (state, flag, Vector3.zero);
			state = "pugong";
			flag = /*!anim.GetBool (state) && */ETCInput.GetButtonDown ("ButtonHit");
			SetAnimation (state, flag, Vector3.zero);
			if (Input.GetKey(KeyCode.Escape)) {
				Application.Quit ();
			}
		}
	}

	[Command]
	protected virtual void CmdSetState(string state, bool flag, Vector3 pos) {
		if (anim) {
			/*anim.SetBool (state, flag);
			GenerateEffect (state, flag, pos);*/
			RpcSetState (state, flag, pos);
		}
	}
		
	[ClientRpc]
	protected virtual void RpcSetState(string state, bool flag, Vector3 pos) {
		if (anim) {
			anim.SetBool (state, flag);
			GenerateEffect (state, flag, pos);
		}
	}

	void GenerateEffect(string state, bool flag, Vector3 pos) {
		if (state == "bianshen") {
			int num = 2;
			SetVisual (!flag);
			/*float sp = 0;*/
			if (flag) {
				//sp = 12;
				Destroy(GameObject.Instantiate (skills [num], transform), 30);
			}/* else {
				sp = 6;
			}
			ChangeSpeed (sp);*/
			return;
		}
		if (state != "run" && state != "pugong" && flag) {
			Debug.Log (pos);
			int num = state[state.Length - 1] - '0' - 1;
				
			GameObject skill_effect = null;
			float x, z;

			switch (state) {
			case "skill1":
				skill_effect = GameObject.Instantiate (skills [num], transform);
				break;
			case "skill2":
				x = pos.x - transform.position.x;
				z = pos.z - transform.position.z;
				pos.x = transform.position.x + x * 10;
				pos.y = 1;
				pos.z = transform.position.z + z * 10;
				transform.LookAt (pos);
						
				skill_effect = GameObject.Instantiate (skills [num], 
						transform.position, 
						transform.rotation);
				var ctrl1 = skill_effect.GetComponent<LifeControl> ();
				ctrl1.move = true;
				ctrl1.target = pos;
				ctrl1.rotate = false;
				break;
			case "skill3":
				x = pos.x - transform.position.x;
				z = pos.z - transform.position.z;
				pos.x = transform.position.x + x * 6;
				pos.y = 0.73f;
				pos.z = transform.position.z + z * 6;
				transform.LookAt (pos);
				skill_effect = GameObject.Instantiate (skills [num], 
					pos, transform.rotation);
				var ctrl2 = skill_effect.GetComponent<LifeControl> ();
				ctrl2.rotate = true;
				ctrl2.target = transform.position;
				ctrl2.move = false;
				break;
			case "skill4":
				transform.LookAt (pos);
				pos.y = 1;	
				skill_effect = GameObject.Instantiate (skills [num], pos, transform.rotation);
				break;
			default:
				break;
			}
			Destroy (skill_effect, life_time[num]);
		}
	}

	void SetVisual(bool is_visual) {
		foreach (var render in GetComponentsInChildren<MeshRenderer>()) {
			render.enabled = is_visual;
		}
		foreach (var render in GetComponentsInChildren<SkinnedMeshRenderer>()) {
			render.enabled = is_visual;
		}
		GetComponent<CapsuleCollider> ().enabled = is_visual;
	}

	void ChangeSpeed(float new_speed) {
		joystick.tmSpeed = new_speed;
	}
}