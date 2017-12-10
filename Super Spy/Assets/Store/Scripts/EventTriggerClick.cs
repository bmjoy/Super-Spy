using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventTriggerClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UIClick()
	{
		GameObject tmp = GameObject.Find ("Package(empty)");
		if(tmp.transform.localPosition.y>0)
			tmp.transform.localPosition=new Vector3(0,0,0);
		
	}
}
