using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkAnimatorController : NetworkBehaviour {
	Animator anim;
	Collider weapon;
	public virtual void Start() {
		anim = GetComponent<Animator> ();
		weapon = GetComponent<HeroInit> ().weaponCollider;

		foreach (var item in anim.runtimeAnimatorController.animationClips) {
			if (!Filter(item.name)) {
				AddEvent (item, "OnAnimationStart", 0);
				AddEvent (item, "OnAnimationEnd", item.length);
			}
		}
	}

	bool Filter(string clipName) {
		string[] disable = { "Idle", "idle", "Run", "run" };
		foreach (var item in disable) {
			if (clipName.Contains(item)) {
				return true;
			}
		}
		return false;
	}

	void OnAnimationStart() {
		weapon.enabled = true;
	}

	void OnAnimationEnd() {
		weapon.enabled = false;
	}

	public void SetAnimation(string state) {
		if (isLocalPlayer) {
			CmdPlay (state);
		}	
	}

	AnimationClip GetClip(string clipName) {
		foreach (var item in anim.runtimeAnimatorController.animationClips) {
			Debug.Log (item.name);
			if (item.name == clipName) {
				return item;
			}
		}
		return null;
	}

	AnimationEvent MakeEvent(string functionName, float time) {
		AnimationEvent evt = new AnimationEvent ();
		evt.functionName = functionName;
		evt.time = time;
		return evt;
	}

	void AddEvent(AnimationClip clip, string functionName, float time) {
		AnimationEvent evt = MakeEvent (functionName, time);
		clip.AddEvent (evt);
	}

	[Command]
	void CmdPlay(string ani) {
		RpcPlay (ani);
	}

	[ClientRpc]
	void RpcPlay(string state) {
		anim.SetTrigger (state);
	}

	protected virtual void Update() {
		if (isLocalPlayer) {
			anim.SetBool ("run", ETCInput.GetAxis ("Vertical") != 0 ||
			ETCInput.GetAxis ("Horizontal") != 0);
		}
	}
}