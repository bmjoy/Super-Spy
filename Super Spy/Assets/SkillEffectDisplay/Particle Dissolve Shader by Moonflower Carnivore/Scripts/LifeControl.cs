using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeControl : MonoBehaviour {
	public float life_time;
	float start_time;
	public bool rotate;
	public bool move;
	public Vector3 target;
	// Use this for initialization
	void Start () {
		start_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - start_time > life_time) {
			Destroy (gameObject);
		} else {
			if (rotate) {
				transform.RotateAround (target, Vector3.up, 10);
			}
			if (move) {
				transform.position = Vector3.Lerp (transform.position, target, 0.05f);
			}
		}
	}
}
