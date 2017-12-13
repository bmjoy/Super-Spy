using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : Check {
	float last_dis;
	float last_time;
	Vector3 origin_target = Vector3.zero;
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
		if (Time.time - last_time > 30.0f) {
			if (last_time == 0) {
				last_time = Time.time;
			} else {
				GameObject.Destroy (gameObject);
				return;
			}
		}

		GameObject obj = FindObjectAroundthePoint (transform.position,
			Vector3.forward, 270f, 90, attack_distance + 2);

		if (obj) {
			SetTarget (obj.transform.position);
		} else {
			SetTarget (origin_target);
		}
		float dis = Vector3.Distance (transform.position, navMeshAgent.destination);
		if (dis > 0 && dis <= attack_distance) {
			transform.LookAt (navMeshAgent.destination);
			anim.SetBool ("attack", !anim.GetBool("attack"));
		} else {
			anim.SetBool ("attack", false);
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
				SetTarget (target);
			}
		}
		last_dis = dis;
	}
		
	public void SetTarget(Vector3 target) {
		if (navMeshAgent == null) {
			navMeshAgent = GetComponent<NavMeshAgent>();
		}
		target.y = transform.position.y;
		if (origin_target == Vector3.zero) {
			origin_target = target;
		}
		navMeshAgent.SetDestination (target);
	}

}