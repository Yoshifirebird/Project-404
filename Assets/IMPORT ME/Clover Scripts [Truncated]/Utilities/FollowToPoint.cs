using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToPoint : MonoBehaviour {

	public Transform target;
	public float positionRate;
	public bool position;
	public float rotationRate;
	public bool rotation;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if(position) transform.position = Vector3.MoveTowards (transform.position, target.position, (Time.deltaTime * positionRate)*Vector3.Distance(transform.position, target.position));
		if (rotation)
			transform.rotation = Quaternion.Slerp (transform.rotation, target.rotation, (Time.deltaTime * rotationRate)*Quaternion.Angle(transform.rotation, target.rotation));

	}
}
