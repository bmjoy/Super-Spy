using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SkillJoystick))]
public class Effect : SkillArea {
	public float cooling_time;
	protected Image mask;
	protected Text time_text;
	protected float cur_time;
	// Use this for initialization
	public virtual void Start () {
		base.Start ();
		joystick.onJoystickUpEvent += PlayEffect;
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
		joystick.onJoystickUpEvent -= PlayEffect;
	}
	
	// Update is called once per frame
	public virtual void Update () {
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

	protected virtual void PlayEffect() {
		if (player) {
			mask.gameObject.SetActive (true);
			mask.fillAmount = 1;
			cur_time = 0;
		}
	}
}