using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BianshenEffect : Effect {
	float remaining_time;
	float life_time;

	public override void SetTarget (GameObject p)
	{
		base.SetTarget (p);
		life_time = p.GetComponent<HeroInit> ().skills [0].lifeTime;
	}

	public override void Update() {
		base.Update ();
		if (remaining_time <= 0) {
			if (player) {
				NetworkSkillController skill_ctrl = player.GetComponent<NetworkSkillController> ();
				skill_ctrl.ShowEffect (SkillType.BianShen, false, Vector3.zero);
			}
		} else {
			remaining_time -= Time.deltaTime;
		}
	}

	void OnGUI() {
		if (remaining_time > 0) {
			string t;
			if (remaining_time < 10f) {
				t = string.Format ("{0:F}", remaining_time);
			} else {
				t = ((int)remaining_time).ToString();
			}
			GUI.Label (new Rect (Screen.width / 2, 25, 100, 50), "<size=30><color=red><b>" + t + "</b></color></size>");
		}
	}
	protected override void PlayEffect ()
	{
		base.PlayEffect ();
		if (player) {
			NetworkSkillController skill_ctrl = player.GetComponent<NetworkSkillController> ();
			skill_ctrl.ShowEffect (SkillType.BianShen, true, Vector3.zero);
			remaining_time = life_time;
		}
	}
}