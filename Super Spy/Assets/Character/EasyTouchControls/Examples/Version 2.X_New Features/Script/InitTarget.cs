﻿using System.Collections;
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
		var skill_buttons = transform.Find ("SkillButtons");
		foreach (var skill in skill_buttons.GetComponentsInChildren<SkillArea>()) {
			skill.player = target.gameObject;
		}
	}
}
