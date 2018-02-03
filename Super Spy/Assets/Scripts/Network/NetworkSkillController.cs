using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSkillController : NetworkBehaviour {
	Skill[] skills;

	void Start() {
		HeroInit init = GetComponent<HeroInit> ();
		skills = init.skills;
	}
	public void ShowEffect(SkillType state, bool flag, Vector3 pos) {
		CmdShowEffect (state, flag, pos);
	}

	[Command]
	void CmdShowEffect(SkillType state, bool flag, Vector3 pos) {
		RpcShowEffect (state, flag, pos);
	}

	[ClientRpc]
	void RpcShowEffect(SkillType state, bool flag, Vector3 pos) {
		GenerateEffect (state, flag, pos);
	}

	void GenerateEffect(SkillType state, bool flag, Vector3 pos) {
		if (state == SkillType.BianShen) {
			SetVisual (!flag);
		}
		if (flag) {
			int num = (int)state;
			GameObject skill_effect = null, skill = skills[num].effect;
			float life_time = skills [num].lifeTime;
			float x, z;
			LifeControl life_ctrl;

			switch (state) {
			case SkillType.Skill1:
				if (gameObject.tag == "Red") {
					skill_effect = GameObject.Instantiate (skill, transform);
				} else {
					skill_effect = GameObject.Instantiate (skill, transform.position, 
						skill.transform.rotation);
				}
				break;
			case SkillType.Skill2:
				transform.LookAt (pos);

				x = pos.x - transform.position.x;
				z = pos.z - transform.position.z;
				skill_effect = GameObject.Instantiate (skill, 
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
			case SkillType.Skill3:
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
				skill_effect = GameObject.Instantiate (skill, 
					pos, skill.transform.rotation);
				if (gameObject.tag == "Blue") {
					life_ctrl = skill_effect.GetComponent<LifeControl> ();
					life_ctrl.target = transform.position;
					life_ctrl.move = false;
					life_ctrl.rotate = true;
				}
				break;
			case SkillType.Skill4:
				transform.LookAt (pos);
				pos.y = skill.transform.position.y;
				skill_effect = GameObject.Instantiate (skill, pos, skill.transform.rotation);
				break;
			case SkillType.BianShen:
				skill_effect = GameObject.Instantiate (skills [num].effect, transform);
				break;
			default:
				break;
			}
			if (skill_effect) {
				Destroy (skill_effect, life_time);
			}
		}
	}

	void SetVisual(bool is_visual) {
		var init = GetComponent<Initialize> ();
		init.OnEnableCheck (is_visual);
		var canvas = GameObject.Find ("Canvas");
		var joystick = canvas.GetComponentInChildren<ETCJoystick> ();

		if (is_visual) {
			joystick.tmSpeed = (init as HeroInit).originSpeed;
		} else {
			joystick.tmSpeed *= 3;
		}
		if (!isLocalPlayer) {
			GetComponentInChildren<Canvas> ().enabled = is_visual;
			foreach (var render in GetComponentsInChildren<MeshRenderer>()) {
				render.enabled = is_visual;
			}
			foreach (var render in GetComponentsInChildren<SkinnedMeshRenderer>()) {
				render.enabled = is_visual;
			}
		}
	}
}
