using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour {
	float last_dis;
	float last_time;
	GameObject origin_target, cur_target;
	Animator anim;
	NavMeshAgent navMeshAgent;

	public float attack_distance;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		navMeshAgent = GetComponent<NavMeshAgent>();
		//origin_target = cur_target = null;
		if (attack_distance == 0) {
			attack_distance = navMeshAgent.stoppingDistance + 1;
		}
		last_dis = 0;
		last_time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float dis = 0;
		if (cur_target) {
			dis = Vector3.Distance (transform.position, cur_target.transform.position);
		}
		if (dis > 0 && dis <= attack_distance) {
			if (last_time == 0) {
				last_time = Time.time;
			} else {
				if (Time.time - last_time >= 20.0f) {
					GameObject.Destroy (gameObject);
					return;
				}
			}
			LookAt ();
			anim.SetBool ("attack", !anim.GetBool ("attack"));
		} else {
			anim.SetBool ("attack", false);
			GameObject obj = FindObjectAroundthePoint (transform.position,
				Vector3.forward, 270f, 90, attack_distance);

			if (obj) {
				SetTarget (obj);
			} else {
				SetTarget (origin_target);
			}
			if (Mathf.Abs (dis - last_dis) < 0.1f * Time.deltaTime) {
				Vector3 target = navMeshAgent.destination;
				switch (Random.Range (0, 4)) {
				case 0:
					target += Vector3.forward * 0.1f;
					break;
				case 1:
					target += Vector3.back * 0.1f;
					break;
				case 2:
					target += Vector3.left * 0.1f;
					break;
				default:
					target += Vector3.right * 0.1f;
					break;
				}
				navMeshAgent.SetDestination (target);
			}
		}
		last_dis = dis;
	}
	void LookAt() {
		Vector3 look_at;
		if (cur_target == origin_target) {
			look_at = origin_target.transform.position;
			look_at.y = transform.position.y;
		} else {
			look_at = navMeshAgent.destination;
		}
		transform.LookAt (look_at);
	}
	public void SetTarget(GameObject target) {
		if (target != null && target != cur_target) {
			if (origin_target == null) {
				origin_target = target;
			}
			cur_target = target;
			Vector3 pos = cur_target.transform.position;
			pos.y = transform.position.y;
			if (navMeshAgent == null) {
				navMeshAgent = GetComponent<NavMeshAgent>();
			}
			navMeshAgent.SetDestination (pos);
		}

	}
	public GameObject FindObjectAroundthePoint(Vector3 point, Vector3 dir, float angle, int rayCount, float distance){ 
     	Vector3 left, right; 
     	float anglefrom = -angle / 2; 
     	float angleTo = angle / 2; 
     	float step = angle / rayCount; 
     	Vector3 temp; 
     	RaycastHit hit;
		 
			
 		for (float an = anglefrom; an <= angleTo; an += step) { 
         	temp = GetRotateVector (Vector3.up*an,dir);
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