using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DayNightController : NetworkBehaviour {
	GameObject image;
	public float day_time, night_time;
	bool is_day;
	//float cur_time;
	// Use this for initialization
	void Start () {
		//cur_time = Time.time;
		is_day = true;

	}

	void Update() {
		if (is_day) {

		}
	}

	[Command]
	void CmdDayNightChange(bool is_night) {
		RpcDayNightChange (is_night);
	}

	[ClientRpc]
	void RpcDayNightChange(bool is_night) {
		LocalChange (is_night);
	}
	void LocalChange(bool is_night) {
		image.SetActive (!is_night);
	}
	public void DayNightChange (bool is_night) {
		if (!isLocalPlayer) {
			return;
		}
		//LocalChange ();
		CmdDayNightChange (is_night);
	}
}
