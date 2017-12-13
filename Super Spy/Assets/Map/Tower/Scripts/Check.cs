using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour {

	public GameObject FindObjectAroundthePoint(Vector3 point, Vector3 dir, float angle, int rayCount, float distance){ 
		float anglefrom = -angle / 2; 
		float angleTo = angle / 2; 
		float step = angle / rayCount; 
		Vector3 temp; 
		RaycastHit hit;
		for (float an = anglefrom; an <= angleTo; an += step) { 
			temp = GetRotateVector (Vector3.up*an,dir);//shexianfangxiang
			Ray ray = new Ray (point,temp); 
			if (Physics.Raycast (ray, out hit, distance)) {
				GameObject obj = hit.transform.gameObject;
				if (obj.tag != gameObject.tag && obj.layer != 9) { 
					return obj;
				}
			} 
		}
		return null; 
	} 
	Vector3 GetRotateVector(Vector3 rogAngle,Vector3 dir){ 
		Vector3 newDir =  Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(rogAngle),Vector3.one) * dir; 
		return newDir.normalized; 
	}   
}
