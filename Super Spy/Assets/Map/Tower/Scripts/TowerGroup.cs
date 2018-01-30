using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerGroup : NetworkBehaviour {
	public GameObject tower;
	public Vector3[] towerPositions;
	public override void OnStartServer ()
	{
		base.OnStartServer ();
		foreach (var pos in towerPositions) {
			GameObject t = GameObject.Instantiate (tower, transform);
			t.transform.localPosition = pos;
			NetworkServer.Spawn (t);
		}
	}
}
