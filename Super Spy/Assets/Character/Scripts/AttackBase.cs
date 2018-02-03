using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackBase : NetworkBehaviour {
	Collider weapon;
	protected int attack_distance;
	float attack_cd;
	int attack_power;
	float _time;
	// Use this for initialization
	protected virtual void Awake () {
		Initialize init = GetComponent<Initialize> ();
		weapon = init.weaponCollider;
		attack_cd = init.attackCd;
		attack_power = init.attackPower;
		_time = 0;
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (!CanAttack()) {
			_time += Time.deltaTime;
			weapon.enabled = false;
		} else {
			weapon.enabled = true;
		}
	}

	GameObject GetRootParent(Transform g) {
		GameObject root = null;
		string untag = "Untagged";
		if (g != null) {
			while (g.parent != null && g.parent.tag == untag) {
				g = g.parent;
			}
			if (g.parent) {
				root = g.parent.gameObject;
			}
		}
		return root;
	}

	void OnTriggerEnter(Collider enemy) {
		if (enemy.GetType() != typeof(CharacterController)) {
			GameObject root = GetRootParent (enemy.transform);
			if (root.tag != gameObject.tag) {
				root.GetComponent<AttackBase> ().Attack (gameObject);
			}
		}
	}

	public virtual bool CanAttack() {
		return _time >= attack_cd;
	}

	public virtual void Attack(GameObject enemy) {
		if (enemy != null) {
			_time = 0;
			AttackBase attack_base = enemy.GetComponent<AttackBase> ();
			if (!attack_base) {
				attack_base = enemy.GetComponentInParent<AttackBase> ();
			}
			attack_base.BeAttacked (gameObject, attack_power);
		}
	}

	public virtual void BeAttacked(GameObject enemy, int power) {
		if (isLocalPlayer) {
			GetComponent<HP> ().UpdateHP (-power);
		}
	}
}
