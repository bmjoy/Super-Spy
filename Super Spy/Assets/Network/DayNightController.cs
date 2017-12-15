using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DayNightController : NetworkBehaviour {
	GameObject image;
	// Use this for initialization
	void Start () {
		var canvas = GameObject.Find ("Canvas");
		image = canvas.transform.Find ("Image").gameObject;
		image.SetActive (false);
		var button = canvas.transform.Find ("Toggle").GetComponent<Toggle> ();
		button.onValueChanged.AddListener(delegate(bool is_night) {
			this.DayNightChange(is_night);
		});
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
