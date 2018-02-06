using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkSkillController : NetworkBehaviour {
	Skill[] skills;

	void Start() {
		HeroInit init = GetComponent<HeroInit> ();
		skills = init.skills;
	}
	public void ShowEffect(SkillType state, Vector3 pos) {
		CmdShowEffect (state, pos);
	}

	[Command]
	void CmdShowEffect(SkillType state, Vector3 pos) {
		RpcShowEffect (state, pos);
	}

	[ClientRpc]
	void RpcShowEffect(SkillType state, Vector3 pos) {
		GenerateEffect (state, pos);
	}

	void GenerateEffect(SkillType state, Vector3 pos) {
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
			SetVisual (false);
			var slider = LobbyManager.s_Singleton.timeSlider;
			slider.onValueChanged.AddListener(delegate(float arg0) {
				if (slider.maxValue != LobbyManager.s_Singleton.timeSlider.GetComponent<DayNightController>().NightTime) {
					Destroy(skill_effect);
					SetVisual(true);
				}
			});
			break;
		default:
			break;
		}
		if (skill_effect) {
			Destroy (skill_effect, life_time);
		}
	}

	void SetVisual(bool is_visual) {
		GetComponent<Initialize> ().OnEnableCheck (is_visual);

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
