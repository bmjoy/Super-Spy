﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Collider))]
public class AttackBase : NetworkBehaviour {
	public int attack_distance;
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
		GetComponent<HP> ().UpdateHP (-power);
	}
}