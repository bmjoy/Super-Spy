using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroInit : NetworkBehaviour {
	public Vector3 origin_position;
	// Use this for initialization
	void Start () {
		transform.position = origin_position;
		if (isLocalPlayer) {
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
}
