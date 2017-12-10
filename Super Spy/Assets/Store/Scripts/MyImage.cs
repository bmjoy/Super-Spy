using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyImage : MonoBehaviour {
	Image image;
	public Sprite remind;
	public Sprite normal;
	public bool small;
	// Use this for initialization
	void Start () {
		small = true;
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (small) {
			transform.localScale = new Vector3 (1, 1, 1);
			image.sprite = normal;
		} else {
			transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			image.sprite = remind;
		}
	}
}
