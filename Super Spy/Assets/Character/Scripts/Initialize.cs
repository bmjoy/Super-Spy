using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Initialize : NetworkBehaviour {
	[Space]
	[Header("Attack Properties")]
	public int maxBlood;
	public GameObject HPBar;
	public int attackDistance;
	public float attackCd;
	public int attackPower;
	public bool isVisual;

	protected T Add<T>() where T : Component {
		return gameObject.AddComponent<T> () as T;
	}

	public virtual void OnEnableAnimator() {
		(Add<NetworkAnimator> () as NetworkAnimator).animator = GetComponent<Animator> ();
		Add<NetworkAnimatorController> ();
	}

	public virtual void OnEnableSkill() {
		Add<NetworkSkillController> ();
	}

	public virtual void OnEnaleAttack() {
		Add<HP>();
	}

	public virtual void OnEnableExplore() {
		Add<FogOfWarExplorer> ().radius = 10;
	}

	public virtual void OnEnableCheck(bool flag) 
	{
		isVisual = flag;
		CapsuleCollider cp = GetComponent<CapsuleCollider> ();
		if (!cp) {
			cp = Add<CapsuleCollider> ();
			cp.center = new Vector3 (0, 1, 0);
			cp.height = 3;
		}
		cp.enabled = flag;
	}
}
