using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITargetPointer : MonoBehaviour {

	public RectTransform uiElement;
	RectTransform uiElementCur;
	public Camera mainCamera;
	public Vector3 offset;

	// Use this for initialization
	void Start () {

		mainCamera = Camera.main;

	}
	public void activate ()	{

		this.enabled = true;
		//print ("Main GUI instance is " + MainGUI.instance);
		uiElementCur = Instantiate (uiElement, MainGUI.instance.transform);
		LateUpdate ();
	}
	// Update is called once per frame
	void LateUpdate () {

		if (uiElementCur) {
			Vector3 input = mainCamera.WorldToViewportPoint (transform.position + offset);

			//input = new Vector2 (input.x / Screen.width, input.y / Screen.height);

			uiElementCur.anchorMax = (Vector2)input;
			uiElementCur.anchorMin = (Vector2)input;

			uiElementCur.gameObject.SetActive (input.z >= 0);
		}
	}



	public void deactivate ()	{

		this.enabled = false;
		Destroy (uiElementCur.gameObject);
	}
}
