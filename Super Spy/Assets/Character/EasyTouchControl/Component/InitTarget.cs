using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitTarget : MonoBehaviour {
	public void SetTarget(Transform target) {
		foreach (var skill in GetComponentsInChildren<SkillEffect>()) {
			skill.player = target.gameObject;
		}

		var bianshen = GetComponentInChildren<BianshenEffect> ();
		if (bianshen) {
			bool is_spy = target.GetComponent<SpyIdentity> ().IsSpy;
			if (is_spy) {
				bianshen.player = target.gameObject;
			} else {
				bianshen.gameObject.SetActive (false);
			}
		}
	}
}
