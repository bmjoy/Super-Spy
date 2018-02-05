using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class BianshenEffect : Effect {
	public override void Start ()
	{
		base.Start ();
		Slider slider = LobbyManager.s_Singleton.timeSlider;
		slider.onValueChanged.AddListener (delegate(float arg0) {
			if (arg0 <= 0) {
				gameObject.SetActive (true);
			} else if (arg0 >= slider.maxValue) {
				gameObject.SetActive (false);
			}
		});
	}

	protected override void PlayEffect ()
	{
		base.PlayEffect ();
		gameObject.SetActive (false);
		Destroy(Camera.main.GetComponent<FogOfWarEffect>());
		if (player) {
			NetworkSkillController skill_ctrl = player.GetComponent<NetworkSkillController> ();
			skill_ctrl.ShowEffect (SkillType.BianShen, Vector3.zero);
		}
	}
}