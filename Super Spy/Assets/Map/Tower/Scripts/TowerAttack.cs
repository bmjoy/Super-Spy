using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class TowerAttack : AutoAttack {
	GameObject bullet;
	SpriteRenderer show;
	string zhenying;
	string blood_tag;
	bool toShow;

	static Dictionary<string, Color> colors;

	public override void Start ()
	{
		base.Start ();
		LobbyManager.s_Singleton.towers [name] = gameObject;
	}

	public override void Update ()
	{
		base.Update ();
		if (isServer) {
			SyncToClients ();
		}
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		bullet = GetComponent<TowerInit> ().bullet;
		OnZhenyingChanged (tag);
		OnBloodEmpty (tag);
		OnShow (false);
		SyncToClients ();
	}

	void SyncToClients() {
		LobbyManager.s_Singleton.UpdateTower (name, zhenying, blood_tag, toShow);
	}

	public override void AutoHit(GameObject enemy) {
		base.AutoHit (enemy);
		OnShow (enemy != null);
		if (enemy) {
			GameObject n = Instantiate(bullet, transform);
			n.GetComponent<Bullet>().InitData(enemy, 8, colors[tag]);
			NetworkServer.Spawn (n);
		}
	}

	public override void BeAttacked(GameObject enemy, int power) {
		base.BeAttacked (enemy, power);
		if (isServer) {
			HP hp = GetComponent<HP> ();
			string enemyTag = enemy.tag;
			if (blood_tag != enemyTag) {
				if (power < hp.curBlood) {
					hp.curBlood -= power;
				} else {
					hp.curBlood = (power - hp.curBlood);
					OnBloodEmpty (enemyTag);
				}
			} else {
				int newblood = hp.curBlood + power;
				if (newblood >= hp.originBlood) {
					hp.curBlood = hp.originBlood;
					OnZhenyingChanged (blood_tag);
				} else {
					hp.curBlood = newblood;
				}
			}
		}
	}

	public void OnBloodEmpty(string value) {
		if (blood_tag != value) {
			blood_tag = value;
			GetComponent<HP> ().UpdateColor (blood_tag);
		}
	}
	public void OnZhenyingChanged(string value) {
		if (zhenying != value) {
			zhenying = value;
			gameObject.tag = zhenying;
			transform.Find ("Walls").tag = zhenying;
			SetTowerMaterial ();
		}
	}

	public void OnShow(bool value) {
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
