using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CharacterAnimationDungeon : MonoBehaviour/*NetworkBehaviour*/ {

	private CharacterController cc;
	private Animation anim;
	int attack;
	// Use this for initialization
	void Start () {
		//if (isLocalPlayer) {
			cc= GetComponent<CharacterController>();
			anim = GetComponentInChildren<Animation>();
			//transform.position = new Vector3 (-30, 3, -32);
			var controller = GameObject.FindWithTag ("GameController");
			if (controller) {
				var joystick = controller.GetComponentInChildren<ETCJoystick> ();
				joystick.cameraLookAt = joystick.axisX.directTransform = joystick.axisY.directTransform = transform;
				var button = controller.GetComponentInChildren<ETCButton> ();
				if (button) {
					button.axis.directTransform = transform;
				}
			}
			attack = 0;
		//}

	}
	
	
	// Wait end of frame to manage charactercontroller, because gravity is managed by virtual controller
	void LateUpdate(){
		//if (isLocalPlayer) {
			if (cc.isGrounded && (ETCInput.GetAxis("Vertical")!=0 || ETCInput.GetAxis("Horizontal")!=0)){
				//cc.enabled = true;
				anim.CrossFade("soldierRun");
			}

			if (cc.isGrounded && ETCInput.GetAxis("Vertical")==0 && ETCInput.GetAxis("Horizontal")==0){
				//cc.enabled = false;
				anim.CrossFade("soldierIdleRelaxed");
			}
			if (ETCInput.GetButton("Button fire")) {
				anim.CrossFade ("soldierFiring");/*
				cc.enabled = false;
				switch (attack) {
				case 0:
					anim.CrossFade ("Attack_standy");
					break;
				case 1:
					anim.CrossFade ("Attack");
					break;
				case 2:
					anim.CrossFade ("Attack01");
					break;
				case 3:
					anim.CrossFade ("Attack02");
					break;
				default:
					break;
				}
				attack = (attack + 1) % 4;*/
			}
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit ();
			}
		//}
	}
}
