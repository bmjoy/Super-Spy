using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleController : MonoBehaviour, IPointerClickHandler{
	public Transform minimap;
	public GameObject point;
	public Sprite minus;
	public Sprite plus;
	public Sprite blank;
	public Sprite origin;

	public void OnPointerClick (PointerEventData eventData) {
		if (minimap.localScale.x == 1) {
			minimap.localScale = new Vector3 (2, 2, 2);
			transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			GetComponent<Image> ().sprite = minus;
			point.GetComponent<MyImage> ().small = false;
		} else {
			minimap.localScale = new Vector3 (1, 1, 1);
			transform.localScale = new Vector3 (1, 1, 1);
			GetComponent<Image> ().sprite = plus;
			point.GetComponent<MyImage> ().small = true;
		}
	}
}
