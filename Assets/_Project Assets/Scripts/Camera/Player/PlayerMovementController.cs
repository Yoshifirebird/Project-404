/*
 * PlayerMovementController.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	// Uncomment these when there are variables in place
	//[Header("Components")]
	CharacterController _Controller;
	Camera _MainCamera;

	[Header("Settings")]
	[SerializeField] float _MovementSpeed = 3;
	[SerializeField] float _GravityMultiplier = 1; 
	
	private void Awake()
	{
		_Controller = GetComponent<CharacterController>();
		_MainCamera = Camera.main;
	}
	
	private void Update()
	{
		HandleMovement();
	}

	private void HandleMovement()
	{
		var mDirection = new Vector3(Input.GetAxis("Horizontal"),
                                     0,
                                     Input.GetAxis("Vertical")).normalized * _MovementSpeed;

		if (mDirection.x == 0 && mDirection.z == 0)
			return;

		mDirection = _MainCamera.transform.TransformDirection(mDirection);
		mDirection.y -= Physics.gravity.y * Time.deltaTime;
		_Controller.Move(mDirection * Time.deltaTime);
	}
}
