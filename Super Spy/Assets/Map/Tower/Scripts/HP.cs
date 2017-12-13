using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour {
	public int blood=10;
	// Use this for initialization
	void Start () {
		
	}
	public  void AddHP(int hp)
	{
		blood += hp;
		Debug.Log ("hp: " + blood);
		if (blood <= 0) {
			GameObject.Destroy(gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
	}
}
