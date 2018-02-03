using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Skill
{
	public GameObject effect;
	public float lifeTime;
	public float CD;
}
public class HeroInit : Initialize {
	[Space]
	[Header("Born Properties")]
	public Vector3 originPosition;
	public Collider weaponCollider;

	[Header("Skill Properties")]
	public Skill[] skills;
	// Use this for initialization
	public override void OnEnaleAttack ()
	{
		base.OnEnaleAttack ();
		base.Add<HeroAttack> ();
	}
	void Awake () {
		transform.localPosition = originPosition;
		OnEnableAnimator ();
		OnEnableSkill ();
		OnEnableCheck (true);
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
