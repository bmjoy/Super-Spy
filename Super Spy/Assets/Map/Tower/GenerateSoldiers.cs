using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateSoldiers : MonoBehaviour {
	public GameObject soldier;
	public GameObject magician;

	float last_time, cur_time;
	Transform[] targets;
	// Use this for initialization
	void Start() {
		cur_time = last_time = 0;
		targets = GameObject.Find ("TowersPosition").GetComponent<Positions>().targets;
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
		GameObject origin_target;
		while (true) {
			int index = Random.Range (0, targets.Length);
			GameObject obj = targets [index].gameObject;
			if (gameObject.tag != obj.tag) {
				origin_target = obj;
				break;
			}
		}

		GameObject ins = null;
		for (int i = 0; i < 3; i++) {
			GameObject new_soldier = null;
			if (ins == null) {
				ins = magician;
			} else {
				ins = soldier;
			}
			new_soldier = (GameObject)Instantiate (ins, transform);
			new_soldier.GetComponent<SoldierInit> ().SetStage (gameObject.tag);
			new_soldier.GetComponent<Controller> ().SetTarget (origin_target);
		}
	}
}
