﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerInit : Initialize {
	public GameObject bullet;
	public GameObject soldier;
	public GameObject magician;
	public Vector3 red_quanshui, blue_quanshui;

	public override void OnEnaleAttack ()
	{
		base.OnEnaleAttack ();
		base.Add<TowerAttack> ();
		//base.Add<GenerateSoldiers> ();
	}

	void Awake () {
		NetworkIdentity id = GetComponent<NetworkIdentity> ();
		if (id.observers == null || id.observers.Count == 0) {
			Debug.Log ("lalala");
			id.RebuildObservers (true);
		}
		OnEnaleAttack ();
	}
}
