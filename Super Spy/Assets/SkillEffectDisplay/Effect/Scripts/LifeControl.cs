using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeControl : MonoBehaviour {
	public bool rotate, move;
	public float life_time;
	public Vector3 target;
	float cur_time;
	// Use this for initialization
	void Start () {
		cur_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - cur_time > life_time) {
			Destroy (gameObject);
		} else {
			if (rotate) {
				transform.RotateAround (target, Vector3.up, 10);
			}
			if (move) {
				transform.position = Vector3.Lerp (transform.position, target, 0.01f);
			}
		}
	}
}
