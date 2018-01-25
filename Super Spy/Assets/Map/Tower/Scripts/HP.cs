using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour {
	public int blood=10;

	public  void AddHP(int hp)
	{
		blood += hp;
		if (blood <= 0) {
			GameObject.Destroy(gameObject);
		}
	}
}
