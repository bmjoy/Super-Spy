using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SkillEffect : Effect {
	public string skill;
	public override void LateUpdate() {
		base.LateUpdate ();
		if (player) {
			NetworkAnimatorController ani_ctrl = player.GetComponent<NetworkAnimatorController> ();
			ani_ctrl.SetAnimation (skill, false);
			NetworkSkillController skill_ctrl = player.GetComponent<NetworkSkillController> ();
			skill_ctrl.ShowEffect (skill, false, Vector3.zero);
		}

	}
	protected override void PlayEffect()
	{
		base.PlayEffect ();
		if (player) {
			NetworkAnimatorController ani_ctrl = player.GetComponent<NetworkAnimatorController> ();
			NetworkSkillController skill_ctrl = player.GetComponent<NetworkSkillController> ();

			Vector3 pos = Vector3.zero;
			switch (areaType) {
			case SkillAreaType.OuterCircle_InnerCircle:
				pos = GetCirclePosition (outerRadius);
				break;
			case SkillAreaType.OuterCircle_InnerCube:
			case SkillAreaType.OuterCircle_InnerSector:
				pos = GetCubeSectorLookAt ();
				break;
			default:
				break;
			}
			ani_ctrl.SetAnimation (skill, true);
			skill_ctrl.ShowEffect (skill, true, pos);
		}
	}

}