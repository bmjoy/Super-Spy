using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comeback : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UIClick()
	{
		GameObject.Find ("Package(empty)").transform.Translate (0, 1000, 0);
	}
}
