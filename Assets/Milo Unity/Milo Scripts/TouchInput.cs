﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour {
	Camera camera;
	public LayerMask TouchInputMask;
	private List<GameObject> touchList = new List<GameObject>();
	private GameObject[] touchesOld;
	private RaycastHit hit;


	void Start() {
		camera = GetComponent<Camera>();
	}
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		if (Input.GetMouseButton(0) ||  Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {

			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear ();

			Ray ray = camera.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, TouchInputMask)) {
					GameObject receiver = hit.transform.gameObject;
					touchList.Add (receiver);

					if (Input.GetMouseButtonDown(0)) {
						receiver.SendMessage ("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
					}

					if (Input.GetMouseButtonUp(0)) {
						receiver.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					}

					if (Input.GetMouseButton(0)) {
						receiver.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					
				}
			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains (g)) {
					g.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
#endif

		if (Input.touchCount > 0) {

			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear ();

			foreach (UnityEngine.Touch touch in Input.touches) {
				Ray ray = camera.ScreenPointToRay (touch.position);

				if (Physics.Raycast (ray, out hit, TouchInputMask)) {
					GameObject receiver = hit.transform.gameObject;
					touchList.Add (receiver);

					if (touch.phase == TouchPhase.Began) {
						receiver.SendMessage ("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
					}

					if (touch.phase == TouchPhase.Ended) {
						receiver.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					}

					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
						receiver.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
					}

					if (touch.phase == TouchPhase.Canceled) {
						receiver.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains (g)) {
					g.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
