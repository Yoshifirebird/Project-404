﻿/*
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
    public Vector2 _Offset = Vector2.one;
}

public class CameraFollow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform _ToFollow;
    [SerializeField] CFVariableHolder[] _DefaultHolders;    // Default View variables
    [SerializeField] CFVariableHolder[] _TopViewHolders;  // Top View variables
    Camera _MainCamera;

    [Header("Settings")]
    [SerializeField] float _FollowSpeed;
    [SerializeField] float _RotationSpeed;
    [SerializeField] float _FOVChangeSpeed;
    [SerializeField] float _AdjustRotationSpeed;

    int _HolderIndex;
    CFVariableHolder _CurrentHolder;
    float _OrbitRadius;
    float _GroundOffset;
    bool _TopView = false;

    CameraFollow()
    {
        // Apply movement/rotation settings
        _FollowSpeed = 5;
        _FOVChangeSpeed = 2;
        _RotationSpeed = 2;
        _AdjustRotationSpeed = 2;

        // Set the default index
        _HolderIndex = 1;
    }

    void Awake()
    {
        // Grab the main camera's 'Camera' component
        _MainCamera = Camera.main;

        if (_DefaultHolders.Length != _TopViewHolders.Length)
        {
            Debug.LogError("Top View holders must have the same length as default holders!");
            Debug.Break();
        }

        // Assign the current holder and the associated variables
        _CurrentHolder = _DefaultHolders[_HolderIndex];
        _OrbitRadius = _CurrentHolder._Offset.x;
        _GroundOffset = _CurrentHolder._Offset.y;
    }

    void Update()
    {
        if (Input.GetButton("RightTrigger"))
            RotateView(_AdjustRotationSpeed);
        else if (Input.GetButton("LeftTrigger"))
            RotateView(-_AdjustRotationSpeed);

        if (Input.GetButton("CameraReset")) ; // Remove the ; when doing this if statement
                                              // Todo: make camera reset back to it's initial position behind the player

        ApplyVariables();

        if (Input.GetButtonDown("ZoomLevel"))
        {
            // Increment the holder index to go to the next zoom level
            _HolderIndex++;
            // Does a basic check to see if we're in top view, and if it is use the alternate holders
            ApplyChangedZoomLevel(_TopView ? _TopViewHolders : _DefaultHolders);
        }

        // Check if the player presses the Camera button
        if (Input.GetButtonDown("CameraAngle"))
        {
            if (_TopView)
            {
                // we're in top view, so change it to the default view
                ApplyChangedZoomLevel(_DefaultHolders);
                _TopView = false;
            }
            else
            {
                // we're not in top view, so change it to the top view
                ApplyChangedZoomLevel(_TopViewHolders);
                _TopView = true;
            }
        }
    }

    void LateUpdate()
    {
        // To stop jittery rotation, we apply the lookat after rotation and other functions have been caleld
        transform.LookAt(_ToFollow.position);
    }

    void ApplyVariables()
    {
        // Smoothly change the orbit radius to the X offset in the current holder
        _OrbitRadius = Mathf.Lerp(_OrbitRadius, _CurrentHolder._Offset.x, _FOVChangeSpeed * Time.deltaTime);
        // Smoothly change the ground offset to the Y offset in the current holder
        _GroundOffset = Mathf.Lerp(_GroundOffset, _CurrentHolder._Offset.y + Player.player.transform.position.y, _FOVChangeSpeed * Time.deltaTime);

        // Calculate the position we're aiming to go to
        Vector3 targetPosition = (transform.position - _ToFollow.transform.position).normalized * Mathf.Abs(_OrbitRadius) + _ToFollow.transform.position;
        targetPosition.y = _GroundOffset;
        // Smoothly change our position towards the targetPosition
        transform.position = Vector3.Lerp(transform.position, targetPosition, _FollowSpeed * Time.deltaTime);

        // Smoothly change the field of view to be the FOV variable in the current holder
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
    }

    void RotateView(float direction)
    {
        // Move the camera right by an offset of 'direction'
        // This works because we call 'LookAt' in LateUpdate
        transform.Translate(Vector3.right * Time.deltaTime * direction);
    }

    void ApplyChangedZoomLevel(CFVariableHolder[] currentHolder)
    {
        // Wrap the holder index
        if (_HolderIndex > currentHolder.Length - 1)
            _HolderIndex = 0;
        // Assign the current holder
        _CurrentHolder = currentHolder[_HolderIndex];
    }
}
