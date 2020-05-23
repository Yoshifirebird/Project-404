using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraTarget : MonoBehaviour {

	public VirtualCamera defaultCam;
	public VirtualCamera targetCam;
	//private VirtualCamera previousCam;
	public Vector3 lastPos;
	public Quaternion lastRot;
	public float lastFov;
	public float interpolation = 1;
	public Camera camera;


	// Use this for initialization
	void Awake () {

		//previousCam = targetCam;
		lastPos = camera.transform.position;
		lastRot = camera.transform.rotation;
		lastFov = camera.fieldOfView;

	}


	public void changeCameras (VirtualCamera target)	{

		//previousCam = targetCam;
		if (targetCam) {
			lastPos = targetCam.transform.position;
			lastRot = targetCam.transform.rotation;
			lastFov = targetCam.fieldOfView;
			targetCam.active = false;
		}
		targetCam = target;
		interpolation = 0;

		targetCam.active = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (targetCam) {
			camera.fieldOfView = Mathf.Lerp (lastFov, targetCam.fieldOfView, interpolation);
			camera.transform.position = Vector3.Lerp (lastPos, targetCam.transform.position, interpolation);
			camera.transform.rotation = Quaternion.Lerp (lastRot, targetCam.transform.rotation, interpolation);
		}



	}
}
