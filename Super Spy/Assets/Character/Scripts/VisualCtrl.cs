using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCtrl : Check {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var obj = FindObjectAroundthePoint (transform.position, Vector3.forward, 360f, 90, 10f);
		if (!isLocalPlayer) {
			SetVisual (obj != null);
		}
	}

	void SetVisual(bool is_visual) {
		foreach (var render in GetComponentsInChildren<SkinnedMeshRenderer>()) {
			render.enabled = is_visual;
		}
		foreach (var render in GetComponentsInChildren<MeshRenderer>()) {
			render.enabled = is_visual;
		}
	}
}
