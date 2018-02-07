using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class BianshenEffect : Effect {
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