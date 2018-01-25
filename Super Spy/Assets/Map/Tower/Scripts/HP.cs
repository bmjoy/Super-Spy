using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityEngine.UI;

public class HP : MonoBehaviour {
	public GameObject HPBar;
	public int blood=10;
	protected int m_originBlood;

	public int originBlood {
		get	{
			return m_originBlood;
		}
	}

	GameObject bar;

	protected virtual void Start() {
		m_originBlood = blood;
		bar = GameObject.Instantiate (HPBar, transform);
		UpdateColor (gameObject.tag);
		UpdateHP (0);
	}

	protected virtual void Update() {
		bar.transform.LookAt (Camera.main.transform);
	}

	public void UpdateHP(int hp) {
		blood += hp;
		if (blood > 0) {
			if (blood > originBlood) {
				blood = originBlood;
			}
			UpdateBar ();
		} else {
			GameObject.Destroy(gameObject);
		}
	}

	public void UpdateBar() {
		bar.GetComponentInChildren<Image> ().fillAmount = blood / (float)originBlood;
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
}
