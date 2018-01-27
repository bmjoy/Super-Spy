using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;
using UnityEngine.UI;

public class HP : NetworkBehaviour {
	public GameObject HPBar;

	public int max_blood = 10;
	[SyncVar (hook = "OnHeathChanged")]
	int blood;

	public int curBlood {
		get	{
			return blood;
		}
		set {
			blood = value;
		}
	}
	public int originBlood {
		get	{
			return max_blood;
		}
	}

	GameObject bar;

	protected virtual void Update() {
		bar.transform.LookAt (Camera.main.transform);
	}

	void OnEnable() {
		bar = GameObject.Instantiate (HPBar, transform);
		UpdateColor (gameObject.tag);
		blood = max_blood;
	}

	[Server]
	public void UpdateHP(int hp) {
		bool died = false;
		blood += hp;
		if (blood > 0) {
			if (blood > max_blood) {
				blood = max_blood;
			}
		} else {
			died = true;
		}
		RpcCheck (died);
	}

	[ClientRpc]
	void RpcCheck(bool died) {
		if (died) {
			Destroy (gameObject);
		}
	}

	public void UpdateBar() {
		bar.GetComponentInChildren<Image> ().fillAmount = blood / (float)max_blood;
	}

	public virtual void UpdateColor(string tag) {
		switch (tag) {
		case "Red":
			bar.GetComponentInChildren<Image> ().color = Color.red;
			break;
		case "Blue":
			bar.GetComponentInChildren<Image> ().color = Color.green;
			break;
		default:
			bar.GetComponentInChildren<Image> ().color = Color.gray;
			break;
		}
	}

	void OnHeathChanged(int value) {
		blood = value;
		UpdateBar ();
	}
}
