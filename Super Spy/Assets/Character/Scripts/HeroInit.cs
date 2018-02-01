﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroInit : Initialize {
	[Header("Born Position")]
	public Vector3 originPosition;

	[Space]
	[Header("Skill Properties")]
	public GameObject[] skills;
	public float[] lifeTimes;
	public float[] skillCDs;
	// Use this for initialization
	public override void OnEnaleAttack ()
	{
		base.OnEnaleAttack ();
		base.Add<HeroAttack> ();
	}
	void Awake () {
		transform.position = originPosition;
		OnEnableAnimator ();
		OnEnableSkill ();
		OnEnableCheck ();
		OnEnaleAttack ();
	}
		
	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
		OnEnableExplore ();
		var canvas = GameObject.Find ("Canvas");
		canvas.GetComponentInChildren<InitTarget> ().SetTarget (transform);
		var joystick = canvas.GetComponentInChildren<ETCJoystick> ();
		if (gameObject.tag == "Blue") {
			canvas.GetComponentInChildren<PolygonCollider2D>().transform.rotation = Quaternion.Euler (0, 0, 180);
			joystick.TurnAndMove = -1;
			joystick.followOffset.z *= -1;
			canvas.GetComponentInChildren<MiniMapCameraManager> ().flag = -1;
		}
		joystick.axisX.directTransform = joystick.axisY.directTransform = joystick.cameraLookAt = transform;
	}
}
