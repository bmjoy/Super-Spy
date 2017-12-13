using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffect : Effect {
	public string skill;

	public override void LateUpdate() {
		base.LateUpdate ();
		if (player) {
			//var cc = player.GetComponent<CharacterController> ();
			var anim = player.GetComponent<Animator> ();
			AnimatorStateInfo state_info = anim.GetCurrentAnimatorStateInfo (0);
			/*if (cc.enabled == false) {
				cc.enabled = state_info.normalizedTime >= 1.0f;
			}*/
			anim.SetBool (skill, false);
		}

	}
	protected override void PlayEffect()
	{
		base.PlayEffect ();
		if (player) {
			var anim = player.GetComponent<Animator> ();
			/*var cc = player.GetComponent<CharacterController> ();
			cc.enabled = false;*/
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
		}
	}

}
