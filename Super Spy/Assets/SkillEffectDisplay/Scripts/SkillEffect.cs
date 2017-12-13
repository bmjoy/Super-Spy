using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffect : SkillArea {
	public string skill;
	public GameObject effect;
	Image mask;
	Text time_text;
	public float cooling_time;
	float cur_time;
	// Use this for initialization
	public override void Start () {
		base.Start ();
		joystick.onJoystickUpEvent += PlaySkill;
		cur_time = 0;
		var m_mask = transform.parent.Find (
			             transform.gameObject.name.Replace ("Skill", "mask"));
		mask = m_mask.GetComponent<Image>();
		mask.gameObject.SetActive (false);
		time_text = m_mask.GetComponentInChildren<Text>();
		time_text.text = "";
	}

	public override void OnDestroy() {
		base.OnDestroy ();
		joystick.onJoystickUpEvent -= PlaySkill;
	}

	void Update() {
		if (mask.fillAmount == 0) {
			time_text.text = "";
			mask.gameObject.SetActive (false);
		} else {
			cur_time += Time.deltaTime;
			float remaining_time = cooling_time - cur_time;
			string t;
			if (remaining_time < 10f) {
				t = string.Format ("{0:F}", remaining_time);
			} else {
				t = ((int)remaining_time).ToString();
			}
			time_text.text = t;
			mask.fillAmount = remaining_time / cooling_time;
		}
	}
	public override void LateUpdate() {
		base.LateUpdate ();
		if (player) {
			var anim = player.GetComponent<Animator> ();
			anim.SetBool (skill, false);
		}

	}
	void PlaySkill()
	{
		if (player/* && mask.fillAmount == 0*/) {
			var anim = player.GetComponent<Animator> ();
			//var cc = player.GetComponent<CharacterController> ();
			//cc.enabled = false;
			anim.SetBool (skill, true);
			Vector3 pos;
			GameObject skill_effect;
			float x, z;
			switch (areaType)
			{
				case SkillAreaType.OuterCircle:
					skill_effect = GameObject.Instantiate (effect, player.transform);
					break;
			case SkillAreaType.OuterCircle_InnerCube:
					pos = GetCubeSectorLookAt ();
					x = pos.x - player.transform.position.x;
					z = pos.z - player.transform.position.z;
					pos.x = player.transform.position.x + x * outerRadius;
					pos.y = 1;
					pos.z = player.transform.position.z + z * outerRadius;
					player.transform.LookAt (pos);
						
					skill_effect = GameObject.Instantiate (effect, 
						player.transform.position, 
						player.transform.rotation);
					var ctrl1 = skill_effect.GetComponent<LifeControl> ();
					ctrl1.move = true;
					ctrl1.target = pos/*+ Vector3.forward*/;
					
					ctrl1.rotate = false;
					break;
				case SkillAreaType.OuterCircle_InnerSector:
					pos = GetCubeSectorLookAt ();
					x = pos.x - player.transform.position.x;
					z = pos.z - player.transform.position.z;
					pos.x = player.transform.position.x + x * outerRadius;
					pos.y = 0.72f;
					pos.z = player.transform.position.z + z * outerRadius;
					player.transform.LookAt (pos);
					skill_effect = GameObject.Instantiate (effect, 
						pos/* + Vector3.forward*/, player.transform.rotation);
					var ctrl2 = skill_effect.GetComponent<LifeControl> ();
					ctrl2.rotate = true;
					ctrl2.target = player.transform.position;
					ctrl2.move = false;
					
					break;
				case SkillAreaType.OuterCircle_InnerCircle:
					pos = GetCirclePosition (outerRadius);
					player.transform.LookAt (pos);
					pos.y = 1;	
					skill_effect = GameObject.Instantiate (effect, pos, player.transform.rotation);
					break;
				default:
					break;
			}
			mask.gameObject.SetActive (true);
			mask.fillAmount = 1;
			cur_time = 0;
		}
	}

}
