using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {
	public Material blue, red;
	// Use this for initialization
	void Start () {
		ChangeStage (transform.gameObject.tag);
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
