using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class XiaoBinController : AttackBase {
	float last_dis;
	Vector3 origin_target = Vector3.zero;
	Animator anim;
	NavMeshAgent navMeshAgent;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		anim = GetComponent<Animator> ();
		navMeshAgent = GetComponent<NavMeshAgent>();
		//origin_target = cur_target = null;
		if (attack_distance == 0) {
			attack_distance = (int)navMeshAgent.stoppingDistance + 1;
		}
		last_dis = 0;
	}

	public override void Attack (GameObject enemy)
	{
		base.Attack (enemy);
		if (enemy) {
			Vector3 look = enemy.transform.position;
			look.y = transform.position.y;
			transform.LookAt (look);
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (!CanAttack()) {
			return;
		}
		GameObject obj = Check.FindObjectAroundthePoint (transform.position, attack_distance + 2, gameObject.tag);

		if (obj) {
			SetTarget (obj.transform.position);
		} else {
			SetTarget (origin_target);
		}
		float dis = Vector3.Distance (transform.position, navMeshAgent.destination);
		if (dis > 0 && dis <= attack_distance) {
			transform.LookAt (navMeshAgent.destination);

			anim.SetBool ("attack", !anim.GetBool("attack"));
			if (obj) {
				this.Attack (obj);
			}
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