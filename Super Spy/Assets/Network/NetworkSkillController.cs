using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSkillController : NetworkBehaviour {
	public GameObject[] skills;
	public float[] life_time;
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
				Destroy(GameObject.Instantiate (skills [num], transform), 30);
			}
		} else {
			if (flag) {
				int num = state[state.Length - 1] - '0';

				GameObject skill_effect = null;
				float x, z;

				switch (state) {
				case "skill1":
					skill_effect = GameObject.Instantiate (skills [num], transform);
					break;
				case "skill2":
					transform.LookAt (pos);
					skill_effect = GameObject.Instantiate (skills [num], 
						transform.position, 
						skills [num].transform.rotation);
					break;
				case "skill3":
					transform.LookAt (pos);
					pos = transform.position;
					pos.y = 0.7f;
					skill_effect = GameObject.Instantiate (skills [num], 
						pos, skills [num].transform.rotation);
					break;
				case "skill4":
					transform.LookAt (pos);
					pos.y = skills [num].transform.position.y;
					skill_effect = GameObject.Instantiate (skills [num], pos, skills [num].transform.rotation);
					break;
				default:
					break;
				}
				Debug.Log ("lalala");
				Destroy (skill_effect, life_time[num]);
			}
		}

	}

	void SetVisual(bool is_visual) {
		if (isLocalPlayer) {
			return;
		}
		foreach (var render in GetComponentsInChildren<MeshRenderer>()) {
			render.enabled = is_visual;
		}
		foreach (var render in GetComponentsInChildren<SkinnedMeshRenderer>()) {
			render.enabled = is_visual;
		}
		GetComponent<CapsuleCollider> ().enabled = is_visual;
	}
}
