using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SkillEffect : Effect {
	static Dictionary<SkillType, string> skillName;
	public override void Start ()
	{
		base.Start ();
		if (skillName == null) {
			skillName = new Dictionary<SkillType, string> ();
			skillName [SkillType.Attack] = "attack";
			skillName [SkillType.Skill1] = "skill1";
			skillName [SkillType.Skill2] = "skill2";
			skillName [SkillType.Skill3] = "skill3";
			skillName [SkillType.Skill4] = "skill4";
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
			ani_ctrl.SetAnimation (skillName[skill]);
			if (skill != SkillType.Attack) {
				skill_ctrl.ShowEffect (skill, true, pos);
			}
		}
	}

}