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
    [SerializeField] CFVariableHolder[] _AlternateAngles;
    Camera _MainCamera;

    [Header("Settings")]
    [SerializeField] float _FollowSpeed = 5f;
    [SerializeField] float _FOVChangeSpeed = 2f;
    [SerializeField] float _RotationSpeed = 2f;

    int _HolderIndex = 1;
    CFVariableHolder _CurrentHolder;
    float _OrbitRadius;
    float _GroundOffset;
    bool _TopView = false;

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

        // Assign each variable so as not to be null on runtime
        _CurrentHolder = _Variables[_HolderIndex];
        _OrbitRadius = _Variables[_Variables.Length - 1]._Offset.z;
        _GroundOffset = _Variables[_Variables.Length - 1]._Offset.y;
    }

    void Update()
    {
        // Rotates the view to the right or left depending on the button the player presses
        if (Input.GetButton("RightTrigger"))
            RotateView(_RotationSpeed);
        else if (Input.GetButton("LeftTrigger"))
            RotateView(-_RotationSpeed);

        if (Input.GetButton("CameraReset"))
        {
            // Todo: make camera reset back to it's initial position behind the player
        }

        // Smoothly interpolate the needed values
        SetVariables();

        // Check if the player presses the Zoom button
        if (Input.GetButtonDown("ZoomLevel"))
        {
            // Increment the variable index
            _HolderIndex++;
            ChangeZoomLevel();
        }

        // Check if the player presses the Camera button
        if (Input.GetButtonDown("CameraAngle"))
        {
            if (!_TopView)
            {
                ChangeZoomLevel();
                _CurrentHolder = _AlternateAngles[0];
                _TopView = true;
            }
            else
            {
                ChangeZoomLevel();
                _OrbitRadius = _AlternateAngles[0]._Offset.z;
                _GroundOffset = _AlternateAngles[0]._Offset.y;
                _TopView = false;
            }
        }
    }

    void SetVariables()
    {
        // Interpolates between the last offsets and the current offsets for a smoother transition
        _OrbitRadius = Mathf.Lerp(_OrbitRadius, _CurrentHolder._Offset.z, _FOVChangeSpeed * Time.deltaTime);
        _GroundOffset = Mathf.Lerp(_GroundOffset, _CurrentHolder._Offset.y, _FOVChangeSpeed * Time.deltaTime);

        // Sets the position according to the values above
        transform.position = (transform.position - _ToFollow.transform.position).normalized * Mathf.Abs(_OrbitRadius) + _ToFollow.transform.position;
        Vector3 newOffset = new Vector3(transform.position.x, _GroundOffset, transform.position.z); // Needed because we can't set the y value by itself
        transform.position = newOffset; // Needed because we can't set the y value on its own

        // Linearly interpolate between the current FOV and the target FOV
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
        // Look at the target position
        transform.LookAt(_ToFollow.position);
    }

    void RotateView(float direction)
    {
        // Transform the camera to the right. Works because the Camera's always looking at the player (using transform.LookAt)
        transform.Translate(Vector3.right * Time.deltaTime * direction);
    }

    void ChangeZoomLevel()
    {
        // Check if the index is bigger than the amount of variables we have
        if (_HolderIndex > (_Variables.Length - 1))
        {
            // Reset it back to 0 to avoid out of bounds errors
            _HolderIndex = 0;
            // Set the offsets to the last value, in this case, the last member in the list of variables
            _OrbitRadius = _Variables[_Variables.Length - 1]._Offset.z;
            _GroundOffset = _Variables[_Variables.Length - 1]._Offset.y;
        }
        else
        {
            // Set the offsets to the last value
            _OrbitRadius = _Variables[_HolderIndex - 1]._Offset.z;
            _GroundOffset = _Variables[_HolderIndex - 1]._Offset.y;
        }

        // Assign the current holder with the variable at the given index
        _CurrentHolder = _Variables[_HolderIndex];
    }
}
