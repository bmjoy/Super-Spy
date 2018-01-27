﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour {

	public static GameObject FindObjectAroundthePoint(Vector3 point, float radius, string tag){ 
		Collider[] res = Physics.OverlapSphere (point, radius);
		foreach (var item in res) {
			if (item.tag != tag && item.gameObject.layer != 9) {
				return item.gameObject;
			}
		}
		return null; 
	} 
	static Vector3 GetRotateVector(Vector3 rogAngle,Vector3 dir){ 
		Vector3 newDir =  Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(rogAngle),Vector3.one) * dir; 
		return newDir.normalized; 
	}   
}
