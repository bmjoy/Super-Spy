using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierInit : MonoBehaviour {
	public Material soldier_red, soldier_blue;
	public Material ball_red, ball_blue;
	public void SetStage(string stage) {
		SetTag (stage);
		switch (stage) {
		case "Red":
			SetSoldierMaterial (soldier_red);
			SetBallMaterial (ball_red);
			break;
		case "Blue":
			SetSoldierMaterial (soldier_blue);
			SetBallMaterial (ball_blue);
			break;
		default:
			break;
		}
	}
	void SetTag(string soldier_tag) {
		gameObject.tag = soldier_tag;
	}
	void SetSoldierMaterial(Material material) {
		transform.GetChild (1).GetComponent<SkinnedMeshRenderer> ().material = material;
	}

	void SetBallMaterial(Material material) {
		transform.Find("Ball").GetComponent<MeshRenderer> ().material = material;
	}
}
