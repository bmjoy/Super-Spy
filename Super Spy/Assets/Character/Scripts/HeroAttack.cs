using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroAttack : AttackBase {
	float m_recovery = 0;
	public GameObject tower;

	public override bool CanAttack ()
	{
		return base.CanAttack () && GetComponent<Animator>().GetBool("attack");
	}

	public override void Attack (GameObject enemy)
	{
		base.Attack (enemy);
		if (enemy) {
			Vector3 look = enemy.transform.position;
			look.y = transform.position.y;
			transform.LookAt (look);
		}
	}

	public override void BeAttacked (GameObject enemy, int power)
	{
		base.BeAttacked (enemy, power);
		m_recovery = Time.time;
	}

	protected override void Update ()
	{
		base.Update ();
		if (this.CanAttack()) {
			GameObject obj = Check.FindObjectAroundthePoint (transform.position, attack_distance, gameObject.tag);
			Attack (obj);
		}
		if (Time.time - m_recovery > 1.0f) {
			m_recovery = Time.time;
			GetComponent<HP> ().UpdateHP (1);
		}
	}
}
