using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiaoBinAttack : AttackBase {
	Animator anim;

	protected override void Start ()
	{
		base.Start ();
		anim = GetComponent<Animator> ();
	}

	public override bool CanAttack ()
	{
		return base.CanAttack () && anim.GetBool("attack");
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
}
