using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitTarget : MonoBehaviour {
	public void SetTarget(Transform target) {
		var joystick = GetComponentInChildren<ETCJoystick> ();
		joystick.cameraLookAt = joystick.axisX.directTransform = joystick.axisY.directTransform = target;
		var button = GetComponentInChildren<ETCButton> ();
		if (button) {
			button.axis.directTransform = target;
		}
	}
}
