using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpyIdentity : NetworkBehaviour {
	bool is_spy;
	// Use this for initialization
	public bool IsSpy {
		get	{
			return is_spy;
		}
	}
	void Awake () {
		is_spy = Identity ();
	}
	
	bool Identity() {
		foreach (var player in GameObject.FindGameObjectsWithTag(gameObject.tag)) {
			if (player.name.Contains("Blade")) {
				if (player.GetComponent<SpyIdentity>().IsSpy) {
					return false;
				}
			}
		}
		return true;
	}
}
