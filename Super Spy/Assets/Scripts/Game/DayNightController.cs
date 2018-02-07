using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DayNightController : NetworkBehaviour, IEventSystemHandler {
	[Space]
	[Header("Day Night Time")]
	public float DayTime;
	public float NightTime;
	public GameObject SpyButton;

	Slider timeSlider;
	bool isNight;
	[System.Serializable]
	public class DayNightEvent : UnityEvent<bool>
	{
		
	}

	[SerializeField]
	DayNightEvent m_OnDayNightChange = new DayNightEvent();
	public DayNightEvent onDayNightChanged {
		get {
			return m_OnDayNightChange;
		}
		set {
			m_OnDayNightChange = value;
		}
	}

	void Awake() {
		timeSlider = GetComponent<Slider>();
		LobbyManager.s_Singleton.dayNightController = this;
	}
	// Use this for initialization
	void Start () {
		GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, 0);
		timeSlider.onValueChanged.AddListener (delegate(float arg0) {
			if (arg0 == timeSlider.minValue) {
				timeSlider.maxValue = NightTime;
				isNight = true;
				m_OnDayNightChange.Invoke(isNight);
			} else if (arg0 == timeSlider.maxValue) {
				timeSlider.value = timeSlider.maxValue = DayTime;
				isNight = false;
				m_OnDayNightChange.Invoke(isNight);
			}
		});

		onDayNightChanged.AddListener (delegate(bool arg0) {
			SpyButton.SetActive (arg0);
			if (arg0) {
				Camera.main.gameObject.AddComponent<FogOfWarEffect>();
			}
			else {
				FogOfWarEffect fow = Camera.main.GetComponent<FogOfWarEffect>();
				if (fow) {
					Destroy(fow);
				}
			}
		});

		if (isServer) {
			timeSlider.value = timeSlider.maxValue;
			StartCoroutine (Timer ());
		}
	}

	void SyncToClients() {
		LobbyManager.s_Singleton.UpdateTime (timeSlider.value, timeSlider.maxValue);
	}

	public void SyncFromServer(float value, float max) {
		if (!isServer) {
			if (timeSlider.maxValue != max) {
				timeSlider.maxValue = max;
			}
			if (timeSlider.value != value) {
				timeSlider.value = value;	
			}
		}
	}

	IEnumerator Timer() {
		while (true) {
			yield return null;
			SyncToClients();
			timeSlider.value += Time.deltaTime * (isNight ? 1 : -1);
		}
	}
}
