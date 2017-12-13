using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : Check {
	public Material blue, red;
	MeshRenderer render;
	// Use this for initialization
	void Start () {
		ChangeStage (transform.gameObject.tag);
		var quan = transform.Find ("quan_hero").gameObject;
		render = quan.GetComponentInChildren<MeshRenderer> ();
		render.enabled = false;
	}

	void Update() {
		GameObject obj = FindObjectAroundthePoint (transform.position, Vector3.forward,
			                 360f, 90, 10f);
		render.enabled = obj != null;
	}
	void ChangeStage(string tag) {
		transform.Find ("Walls").tag = tag;
		switch (tag) {
		case "Blue":
			SetTowerMaterial (blue);
			break;
		case "Red":
			SetTowerMaterial (red);
			break;
		default:
			break;
		}
	}

	void SetTowerMaterial(Material material) {
		string[] str = {"Banners", "Crystal", "Tower_Gray"};
		foreach (var s in str) {
			transform.Find(s).GetComponent<MeshRenderer> ().material = material;
		}
	}
}
