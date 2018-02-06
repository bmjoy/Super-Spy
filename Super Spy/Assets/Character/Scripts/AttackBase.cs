using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackBase : NetworkBehaviour {
	int attack_power;
	// Use this for initialization
	protected virtual void Awake () {
		Initialize init = GetComponent<Initialize> ();
		attack_power = init.attackPower;
	}

	public virtual void OnTriggerEnter(Collider enemy) {
		if (enemy.GetType() != typeof(CharacterController) && GetComponent<Initialize>().isVisual) {
			Transform root = Check.GetRootParent (enemy.transform);
			if (root && root != transform) {
				root.GetComponent<AttackBase> ().Attack (gameObject);
			}
		}
	}
		
	public virtual void Attack(GameObject enemy) {
		if (enemy != null) {
			AttackBase attack_base = enemy.GetComponent<AttackBase> ();
			if (!attack_base) {
				attack_base = enemy.GetComponentInParent<AttackBase> ();
			}
			attack_base.BeAttacked (gameObject, attack_power);
		}
	}

	public virtual void BeAttacked(GameObject enemy, int power) {
		
	}
}
