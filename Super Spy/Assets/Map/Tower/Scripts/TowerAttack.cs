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


	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		ChangeStage (tag);
		zhenying = blood_tag = tag;
		show = transform.Find ("rang").GetComponent<SpriteRenderer> ();
		show.enabled = false;
		bullet = GetComponent<TowerInit> ().bullet;
	}
		
	protected override void Update()
	{
		if (!isServer) {
			return;
		}
		base.Update ();
		if (CanAttack()) //超guo冷却cd就攻ji，并重置jishi
		{
			GameObject mAttackTarget = Check.FindObjectAroundthePoint (transform.position, 6f, tag);
			Attack (mAttackTarget);
		}
	}

	[Server]
	public override void BeAttacked(GameObject enemy, int power) {
		HP hp = GetComponent<HP> ();
		if (blood_tag != enemy.tag) {
			if (power <= hp.curBlood) {
				hp.curBlood -= power;
			} else {
				hp.curBlood = power - hp.curBlood;
				blood_tag = enemy.tag;
			}
		} else {
			hp.curBlood += power;
			if (hp.curBlood >= hp.originBlood) {
				hp.curBlood = hp.originBlood;
				zhenying = blood_tag;
			}
		}
	}
		
	public override void Attack(GameObject enemy)
	{
		base.Attack (enemy);
		bool toshow = false;
		if (enemy) {
			toshow = true;
			GameObject n = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y+6, transform.position.z), transform.rotation);
			n.GetComponent<Bullet>().InitData(enemy, 8, 2);
			NetworkServer.Spawn (n);
		}
		show.enabled = toshow;
	}

	void OnBloodEmpty(string value) {
		blood_tag = value;
		GetComponent<HP> ().UpdateColor (blood_tag);
	}
	void OnZhenyingChanged(string value) {
		zhenying = value;
		ChangeStage (zhenying);
	}
		
	public void ChangeStage(string zhenying) {
		gameObject.tag = zhenying;
		transform.Find ("Walls").tag = zhenying;
		switch (zhenying) {
		case "Blue":
			SetTowerMaterial (Color.blue);
			break;
		case "Red":
			SetTowerMaterial (Color.red);
			break;
		case "Gray":
			SetTowerMaterial (Color.gray);
			break;
		default:
			break;
		}

	}

	void SetTowerMaterial(Color color) {
		string[] str = {"Banners", "Crystal", "Tower_Gray"};
		foreach (var s in str) {
			transform.Find(s).GetComponent<MeshRenderer> ().material.color = color;
		}
	}
}
