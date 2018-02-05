﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GenerateSoldiers : NetworkBehaviour {
	GameObject soldier;
	GameObject magician;
	Vector3 red_quanshui, blue_quanshui;

	float last_time, cur_time;

	// Use this for initialization
	[Server]
	void Start() {
		cur_time = last_time = 0;
		TowerInit init = GetComponent<TowerInit> ();
		soldier = init.soldier;
		magician = init.magician;
		red_quanshui = init.red_quanshui;
		blue_quanshui = init.blue_quanshui;
		Generate ();
	}
	// Update is called once per frame
	[Server]
	void Update () {
		cur_time = Time.time;
		if (cur_time - last_time >= 30.0f) {
			Generate ();
			last_time = cur_time;
		}
	}

	void Generate() {
		GameObject ins = null;
		Vector3 target = GetEnermyQuanShui (gameObject.tag);
		for (int i = 0; i < 3; i++) {
			if (ins == null) {
				ins = magician;
			} else {
				ins = soldier;
			}
			GameObject new_soldier = (GameObject)Instantiate (ins, transform);
			new_soldier.GetComponent<SoldierInit> ().SetStage (gameObject.tag);
			new_soldier.GetComponent<XiaoBinController> ().SetTarget (target);
		}
	}

	Vector3 GetEnermyQuanShui(string tag) {
		if (tag == "Red") {
			return blue_quanshui;
		} else if (tag == "Blue") {
			return red_quanshui;
		} else {
			int i = Random.Range (0, 2);
			if (i == 0) {
				return red_quanshui;
			} else {
				return blue_quanshui;
			}
		}
	}
}
