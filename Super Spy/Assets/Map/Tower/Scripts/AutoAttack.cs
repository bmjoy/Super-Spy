using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : AttackBase {
	float attackCD;
	float attackDistance;
	float _time;
	// Use this for initialization
	public virtual void Start () {
		_time = 0;
		Initialize init = GetComponent<Initialize> ();
		attackCD = init.attackCd;
		attackDistance = init.attackDistance;
	}

	public bool CanAttack() {
		return _time >= attackCD;
	}

	public virtual void AutoHit(GameObject enemy) {
		if (enemy) {
			_time = 0;
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (isServer) {
			if (CanAttack()) {
				AutoHit (Check.FindObjectAroundthePoint (transform.position, attackDistance, tag));
			} else {
				_time += Time.deltaTime;
			}
		}
	}
}
