using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaoController : MonoBehaviour {
	void OnCollisionEnter(Collision obj) {
		Debug.Log ("enter");
	}

	void OnCollisionExit(Collision obj) {
		Debug.Log ("exit");
	}
}
