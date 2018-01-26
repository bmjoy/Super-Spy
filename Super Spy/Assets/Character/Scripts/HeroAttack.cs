using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : XiaoBinAttack {
	float m_recovery = 0;

	public override void BeAttacked (GameObject enemy, int power)
	{
		base.BeAttacked (enemy, power);
		m_recovery = Time.time;
	}

	protected override void Update ()
	{
		base.Update ();
		if (Time.time - m_recovery > 1.0f) {
			m_recovery = Time.time;
			GetComponentInParent<HP> ().UpdateHP (1);
		}
	}
}
