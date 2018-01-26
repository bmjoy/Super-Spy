using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour {
	public float attack_distance;
	public float attack_cd;
	public int attack_power;
	float _time;
	// Use this for initialization
	protected virtual void Start () {
		_time = 0;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (!CanAttack()) {
			_time += Time.deltaTime;
		} else {
			GameObject mAttackTarget = Check.FindObjectAroundthePoint (transform.position, attack_distance, gameObject.tag);
			Attack (mAttackTarget);
		}
	}
		
	public virtual void OnTriggerStay(Collider other) {
		AttackBase attack = other.GetComponentInParent<AttackBase> ();
		GameObject obj = attack.gameObject;
		if (obj.tag != gameObject.tag) {
			if (attack.CanAttack()) {
				attack.Attack (gameObject);
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
				attack_base = enemy.GetComponentInChildren<AttackBase> ();
			}
			if (!attack_base) {
				attack_base = enemy.GetComponentInParent<AttackBase> ();
			}
			attack_base.BeAttacked (gameObject, attack_power);
		}
	}

	public virtual void BeAttacked(GameObject enemy, int power) {
		HP hp = GetComponent<HP> ();
		if (!hp) {
			hp = GetComponentInParent<HP> ();
		}
		hp.UpdateHP (-power);
	}
}
