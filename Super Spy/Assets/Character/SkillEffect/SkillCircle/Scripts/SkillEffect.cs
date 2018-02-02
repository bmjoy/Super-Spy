using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SkillEffect : Effect {
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
			ani_ctrl.SetAnimation (skillName[(int)skill]);
			if (skill != SkillType.Attack) {
				skill_ctrl.ShowEffect (skill, true, pos);
			}
		}
	}

}