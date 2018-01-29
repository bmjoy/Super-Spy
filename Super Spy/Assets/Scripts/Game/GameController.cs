using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	float last_esc_time = 0;
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (last_esc_time == 0 || Time.time - last_esc_time > 1.0f) {
				last_esc_time = Time.time;
			} else {
				Application.Quit ();
			}
		}
	}
}
