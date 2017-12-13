using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BianshenEffect : Effect {
	float remaining_time;
	float speed;
	ETCJoystick speed_ctrl;

	public override void Start () {
		base.Start ();
		var canvas = GameObject.Find ("Canvas");
		speed_ctrl = canvas.transform.Find ("Joystick").GetComponent<ETCJoystick> ();
		speed = speed_ctrl.tmSpeed;
	}
	protected virtual void Update() {
		base.Update ();
		if (remaining_time <= 0) {
			SetVisual (true);
			ChangeSpeed (speed);
		} else {
			remaining_time -= Time.deltaTime;
		}
	}

	void OnGUI() {
		if (remaining_time > 0) {
			string t;
			if (remaining_time < 10f) {
				t = string.Format ("{0:F}", remaining_time);
			} else {
				t = ((int)remaining_time).ToString();
			}
			GUI.Label (new Rect (Screen.width / 2, 25, 100, 50), "<size=30><color=red><b>" + t + "</b></color></size>");
		}
	}
	protected override void PlayEffect ()
	{
		base.PlayEffect ();
		if (player) {
			SetVisual (false);
			ChangeSpeed (2 * speed);
			var bianshen_effect = GameObject.Instantiate (effect, player.transform);
			var life_ctrl = bianshen_effect.GetComponent<LifeControl> ();
			remaining_time = life_ctrl.life_time;
			life_ctrl.move = life_ctrl.rotate = false;
		}
	}

	void SetVisual(bool is_visual) {
		if (player) {
			foreach (var render in player.GetComponentsInChildren<MeshRenderer>()) {
				render.enabled = is_visual;
			}
			foreach (var render in player.GetComponentsInChildren<SkinnedMeshRenderer>()) {
				render.enabled = is_visual;
			}
			player.GetComponent<CapsuleCollider> ().enabled = is_visual;
		}
	}

	void ChangeSpeed(float new_speed) {
		if (player) {
			speed_ctrl.tmSpeed = new_speed;
		}
	}
}
