using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour {
	static GameObject GetRootParent(Transform child) {
		while (child.parent && child.parent.tag != "Untagged") {
			child = child.parent;
		}
		return child.gameObject;
	}

	public static GameObject FindObjectAroundthePoint(Vector3 point, float radius, string tag){ 
		Collider[] colls = Physics.OverlapSphere (point, radius, 11);
		foreach (var item in colls) {
			var root = GetRootParent (item.transform);

			if (root.tag != tag) {
				return root;
			}
		}
		return null; 
	} 
	static Vector3 GetRotateVector(Vector3 rogAngle,Vector3 dir){ 
		Vector3 newDir =  Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(rogAngle),Vector3.one) * dir; 
		return newDir.normalized; 
	}   
}
