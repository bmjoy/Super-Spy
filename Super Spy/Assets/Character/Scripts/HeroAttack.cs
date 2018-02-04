using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroAttack : AttackBase {
	float m_recovery = 0;

	public override void Attack (GameObject enemy)
	{
		base.Attack (enemy);
		if (isLocalPlayer && enemy) {
			Debug.Log (enemy);
			Vector3 look = enemy.transform.position;
			look.y = transform.position.y;
			transform.LookAt (look);
		}
	}

	public override void BeAttacked (GameObject enemy, int power)
	{
		base.BeAttacked (enemy, power);
		if (isLocalPlayer) {
			GetComponent<HP> ().UpdateHP (-power);
			m_recovery = Time.time;
		}
	}

	void Update ()
	{
		if (isLocalPlayer) {
			if (Time.time - m_recovery > 1.0f) {
				m_recovery = Time.time;
				GetComponent<HP> ().UpdateHP (1);
			}
		}
	}
}
