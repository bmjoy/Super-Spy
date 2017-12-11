using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : SkillArea {
	public string skill;
	// Use this for initialization
	public override void Start () {
		base.Start ();
		joystick.onJoystickUpEvent += PlaySkill;
	}

	public override void OnDestroy() {
		base.OnDestroy ();
		joystick.onJoystickUpEvent -= PlaySkill;
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
		if (player) {
			var anim = player.GetComponent<Animator> ();
			anim.SetBool (skill, true);
		}
	}

}
