using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroInit : Initialize {
	public Vector3 originPosition;
	public GameObject[] skills;
	public float[] lifeTimes;
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
		canvas.transform.Find ("SkillButtons").GetComponent<InitTarget> ().SetTarget (transform);
		var joystick = canvas.GetComponentInChildren<ETCJoystick> ();
		if (gameObject.tag == "Blue") {
			canvas.transform.Find ("Minimap/MiniMapBg/Mask/Bg").rotation = Quaternion.Euler (0, 0, 180);
			joystick.TurnAndMove = -1;
			joystick.followOffset.z *= -1;
			canvas.GetComponentInChildren<MiniMapCameraManager> ().flag = -1;
		}
		joystick.axisX.directTransform = joystick.axisY.directTransform = joystick.cameraLookAt = transform;
	}
}
