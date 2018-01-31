using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerAttack : AttackBase {
	GameObject bullet;
	SpriteRenderer show;
	[SyncVar (hook = "OnZhenyingChanged")]
	string zhenying;
	[SyncVar (hook = "OnBloodEmpty")]
	string blood_tag;
	[SyncVar (hook = "OnShow")]
	bool toShow;

	static Dictionary<string, Color> colors;

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		bullet = GetComponent<TowerInit> ().bullet;
		zhenying = blood_tag = tag;
		toShow = false;
	}

	protected override void Update()
	{
		base.Update ();
		if (isServer) {
			if (CanAttack()) //超guo冷却cd就攻ji，并重置jishi
			{
				GameObject mAttackTarget = Check.FindObjectAroundthePoint (transform.position, 6f, tag);
				Attack (mAttackTarget);
			}
		}
	}

	public override void BeAttacked(GameObject enemy, int power) {
		HP hp = GetComponent<HP> ();
		if (blood_tag != enemy.tag) {
			if (power < hp.curBlood) {
				hp.UpdateHP (-power);
			} else {
				hp.curBlood = (power - hp.curBlood);
				blood_tag = enemy.tag;
			}
		} else {
			hp.UpdateHP (power);
			if (hp.curBlood >= hp.originBlood) {
				zhenying = blood_tag;
			}
		}
	}
		
	public override void Attack(GameObject enemy)
	{
		base.Attack (enemy);
		toShow = enemy != null;
		if (enemy) {
			GameObject n = Instantiate(bullet, transform);
			n.GetComponent<Bullet>().InitData(enemy, 8, 2);
		}
	}

	void OnBloodEmpty(string value) {
		blood_tag = value;
		GetComponent<HP> ().UpdateColor (blood_tag);
	}
	void OnZhenyingChanged(string value) {
		zhenying = value;
		gameObject.tag = zhenying;
		transform.Find ("Walls").tag = zhenying;
		SetTowerMaterial ();
	}

	void OnShow(bool value) {
		if (toShow != value) {
			if (show == null) {
				show = transform.Find ("rang").GetComponent<SpriteRenderer> ();
			}
			show.enabled = toShow = value;
		}
	}
		
	void SetTowerMaterial() {
		string[] str = {"Banners", "Crystal", "Tower_Gray"};
		if (colors == null) {
			colors = new Dictionary<string, Color> ();
			colors ["Gray"] = Color.gray;
			colors ["Red"] = Color.red;
			colors ["Blue"] = Color.blue;
		}
		Color color = colors [zhenying];
		foreach (var s in str) {
			transform.Find(s).GetComponent<MeshRenderer> ().material.color = color;
		}
	}
}
