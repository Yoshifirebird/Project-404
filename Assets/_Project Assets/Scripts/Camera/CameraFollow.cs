/*
 * CameraFollow.cs
 * Created by: Ambrosia
 * Created on: 6/2/2020 (dd/mm/yy)
 * Created for: needing the camera to follow the player and alter it's variables on a button press
 */

using UnityEngine;

[System.Serializable]
public class CFVariableHolder
{
	public float _FOV = 75f;
	public Vector3 _Offset = Vector3.one;
}

public class CameraFollow : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Transform _ToFollow;
	[SerializeField] CFVariableHolder[] _Variables;
	Camera _MainCamera;

	[Header("Settings")]
	[SerializeField] float _FollowSpeed = 5f;
	[SerializeField] float _FOVChangeSpeed = 2f;

	CFVariableHolder _CurrentHolder;
	int _HolderIndex = 0;

	void Awake()
	{
		// Grab the main camera's 'Camera' component
		_MainCamera = Camera.main;

		// Check if the '_Variables' variable wasn't assigned
		if (_Variables == null)
		{
			Debug.LogError("'Variables' wasn't assigned, and as a result is null.");
			// Break from playing as we've prevented another error down the line 
			Debug.Break();
		}

		// Assign the current holder so as not to be null on runtime
		_CurrentHolder = _Variables[_HolderIndex];
	}

	void Update()
	{
		// Smoothly interpolate the needed values
		SetVariables();

		// Check if the player presses the R key
		if (Input.GetKeyDown(KeyCode.R))
		{
			// Increment the variable index
			_HolderIndex++;
			// Check if the index is bigger than the amount of variables we have
			if (_HolderIndex > (_Variables.Length - 1))
				// Reset it back to 0 to avoid errors
				_HolderIndex = 0;
			 
			// Assign the current holder with the variable at the given index
			_CurrentHolder = _Variables[_HolderIndex];
		}
	}

	void SetVariables()
	{
		// Linearly interpolate between the current position and the target position with an added offset
		transform.position = Vector3.Lerp(transform.position, _ToFollow.position + _CurrentHolder._Offset, _FollowSpeed * Time.deltaTime);
		// Linearly interpolate between the current FOV and the target FOV
		_MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
		// Look at the target position
		transform.LookAt(_ToFollow.position);
	}
}
