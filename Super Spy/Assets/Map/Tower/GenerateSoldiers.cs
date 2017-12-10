using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateSoldiers : MonoBehaviour {
	public GameObject soldier;
	public GameObject magician;
	public Vector3 red_quanshui, blue_quanshui;

	float last_time, cur_time;

	// Use this for initialization
	void Start() {
		cur_time = last_time = 0;
		Generate ();
	}
	// Update is called once per frame
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
				new_soldier.GetComponent<Controller> ().SetTarget (target);
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
