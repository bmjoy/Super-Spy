using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerAttack : AttackBase {
	public Material blue, red, gray;
	public Bullet bullet;
	SpriteRenderer show;
	/*[SyncVar (hook = "OnZhenyingChanged")]
	string zhenying;*/
	[SyncVar (hook = "OnBloodEmpty")]
	string blood_tag;


	// Use this for initialization
	protected override void Start () {
		base.Start ();
		/*zhenying =*/ blood_tag = tag;
		show = transform.Find ("rang").GetComponent<SpriteRenderer> ();
		show.enabled = false;
	}

	/*[Server]
	protected override void Update()
	{
		base.Update ();
		if (CanAttack()) //超guo冷却cd就攻ji，并重置jishi
		{
			GameObject mAttackTarget = Check.FindObjectAroundthePoint (transform.position, 6f, tag);
			this.Attack (mAttackTarget);
		}
	}*/

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
				//zhenying = blood_tag;
			}
		}
	}

	public override void Attack(GameObject enemy)
	{
		attack_power = 0;
		base.Attack (enemy);
		if (enemy == null) {
			show.enabled = false; 
		} else {
			show.enabled = true; 
			Bullet n = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y+6, transform.position.z), transform.rotation);
			n.InitData(enemy, 8, 2);
		}
	}

	void OnBloodEmpty(string value) {
		blood_tag = value;
		GetComponent<HP> ().UpdateColor (blood_tag);
	}
	/*void OnZhenyingChanged(string value) {
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
	}*/
}
