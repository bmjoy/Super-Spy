using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitTarget : MonoBehaviour {
	public void SetTarget(Transform target) {
		foreach (var skill in GetComponentsInChildren<SkillEffect>()) {
			skill.player = target.gameObject;
		}
		GetComponentInChildren<BianshenEffect> ().player = target.gameObject;
	}
}
