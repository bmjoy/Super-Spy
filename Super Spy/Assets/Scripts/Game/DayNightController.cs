using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class DayNightController : NetworkBehaviour {
	[Space]
	[Header("Day Night Time")]
	public float DayTime;
	public float NightTime;

	Slider timeSlider;
	int isDay;
	FogOfWarEffect fow;

	void Awake() {
		LobbyManager.s_Singleton.timeSlider = timeSlider = GetComponent<Slider>();
	}
	// Use this for initialization
	void Start () {
		GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, 0);
		isDay = 1;
		fow = null;
		timeSlider.onValueChanged.AddListener (delegate(float arg0) {
			if (arg0 <= 0) {
				timeSlider.maxValue = NightTime;
				isDay = -1;
				fow = Camera.main.gameObject.AddComponent<FogOfWarEffect> ();
			} else if (arg0 >= timeSlider.maxValue) {
				timeSlider.value = timeSlider.maxValue = DayTime;
				isDay = 1;
				if (fow) {
					Destroy (fow);
				}
			}
		});
		timeSlider.value = timeSlider.maxValue;
	}

	void SyncToClients() {
		LobbyManager.s_Singleton.UpdateTime (timeSlider.value, timeSlider.maxValue);
	}
	
	// Update is called once per frame
	void Update () {
		if (isServer) {
			timeSlider.value -= Time.deltaTime * isDay;
			SyncToClients();
		}
	}
}
