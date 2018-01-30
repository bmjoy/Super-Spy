using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSkillController : NetworkBehaviour {
	GameObject[] skills;
	float[] life_time;

	void Start() {
		HeroInit init = GetComponent<HeroInit> ();
		skills = init.skills;
		life_time = init.lifeTimes;
	}
	public void ShowEffect(string state, bool flag, Vector3 pos) {
		CmdShowEffect (state, flag, pos);
	}

	[Command]
	void CmdShowEffect(string state, bool flag, Vector3 pos) {
		RpcShowEffect (state, flag, pos);
	}

	[ClientRpc]
	void RpcShowEffect(string state, bool flag, Vector3 pos) {
		GenerateEffect (state, flag, pos);
	}

	void GenerateEffect(string state, bool flag, Vector3 pos) {
		if (state == "bianshen") {
			int num = 0;
			SetVisual (!flag);
			if (flag) {
				Destroy(GameObject.Instantiate (skills [num], transform), life_time[num]);
			}
		} else {
			if (flag) {
				int num = state[state.Length - 1] - '0';

				GameObject skill_effect = null;
				float x, z;
				LifeControl life_ctrl;

				switch (state) {
				case "skill1":
					if (gameObject.tag == "Red") {
						skill_effect = GameObject.Instantiate (skills [num], transform);
					} else {
						skill_effect = GameObject.Instantiate (skills [num], transform.position, 
							skills [num].transform.rotation);
					}
					break;
				case "skill2":
					transform.LookAt (pos);

					x = pos.x - transform.position.x;
					z = pos.z - transform.position.z;
					skill_effect = GameObject.Instantiate (skills [num], 
						transform.position, 
						transform.rotation);
					pos.x += x * 5;
					pos.y = 3;
					pos.z += z * 5;
					life_ctrl = skill_effect.GetComponent<LifeControl> ();
					life_ctrl.target = pos;
					life_ctrl.move = true;
					life_ctrl.rotate = false;
					break;
				case "skill3":
					transform.LookAt (pos);
					if (gameObject.tag == "Red") {
						pos = transform.position;
					} else {
						x = pos.x - transform.position.x;
						z = pos.z - transform.position.z;
						pos.x += x * 5;
						pos.z += z * 5;
					}
					pos.y = 0.7f;
					skill_effect = GameObject.Instantiate (skills [num], 
						pos, skills [num].transform.rotation);
					if (gameObject.tag == "Blue") {
						life_ctrl = skill_effect.GetComponent<LifeControl> ();
						life_ctrl.target = transform.position;
						life_ctrl.move = false;
						life_ctrl.rotate = true;
					}
					break;
				case "skill4":
					transform.LookAt (pos);
					pos.y = skills [num].transform.position.y;
					skill_effect = GameObject.Instantiate (skills [num], pos, skills [num].transform.rotation);
					break;
				default:
					break;
				}
				Destroy (skill_effect, life_time[num]);
			}
		}

	}

	void SetVisual(bool is_visual) {
		GetComponent<CapsuleCollider> ().enabled = is_visual;
		if (!isLocalPlayer) {
			foreach (var render in GetComponentsInChildren<MeshRenderer>()) {
				render.enabled = is_visual;
			}
			foreach (var render in GetComponentsInChildren<SkinnedMeshRenderer>()) {
				render.enabled = is_visual;
			}
		}
	}
}
