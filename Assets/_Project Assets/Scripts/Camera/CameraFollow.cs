/*
 * CameraFollow.cs
 * Created by: Neo, Ambrosia
 * Created on: 6/2/2020 (dd/mm/yy)
 * Created for: following a target with incrementable offset and field of view
 */

using System.Collections;
using UnityEngine;

[System.Serializable]
public class CameraHolder
{
    public float _FOV = 75f;
    public Vector2 _Offset = Vector2.one;
}

[RequireComponent(typeof(AudioSource))]
public class CameraFollow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CameraHolder[] _DefaultHolders;
    [SerializeField] CameraHolder[] _TopViewHolders;
    Camera _MainCamera;

    [Header("Audio")]
    [SerializeField] AudioClip _ChangeZoomAudio;

    [Header("Movement / Camera Specific")]
    [SerializeField] float _FollowSpeed;
    [SerializeField] float _OrbitChangeSpeed;
    [SerializeField] float _FOVChangeSpeed;

    [Header("Movement Correction")]
    [SerializeField] float _HeightChangeSpeed;
    [SerializeField] float _DistForHeightChange;  
    [SerializeField] float _EasingHeightOffset; // The offset of the sphere used,
    [SerializeField] float _HeightSphereRadius; // The radius of the sphere used to check if there's a platform higher than what we're currently on

    [Header("Rotation")]
    [SerializeField] float _LookAtRotationSpeed;

    [Header("Controlled Rotation")]
    [SerializeField] float _RotateAroundSpeed;
    [SerializeField] float _TriggerRotationSpeed;
    [SerializeField] float _CameraResetLength;
    [SerializeField] float _CameraResetSpeed;

    [Header("Miscellaneous")]
    [SerializeField] LayerMask _MapLayer;

    CameraHolder _CurrentHolder;
    AudioSource _AudioSource;
    Transform _PlayerPosition;
    float _OrbitRadius;
    float _GroundOffset;
    float _CurrentRotation;
    int _HolderIndex;
    bool _TopView = false;

    CameraFollow()
    {
        // Movement / Camera Specific
        _FollowSpeed = 5;
        _FOVChangeSpeed = 2;
        _OrbitChangeSpeed = 2;

        // Movement Correction
        _HeightChangeSpeed = 2;
        _DistForHeightChange = Mathf.Infinity;
        _EasingHeightOffset = 2.5f;
        _HeightSphereRadius = 2;

        // Rotation
        _LookAtRotationSpeed = 5;

        // Controlled Rotation
        _RotateAroundSpeed = 4;
        _CameraResetSpeed = 5;
        _CameraResetLength = 2;
        _TriggerRotationSpeed = 2;

        // Non-exposed
        _CurrentRotation = 0;
    }

    void Awake()
    {
        _MainCamera = Camera.main;
        _PlayerPosition = Player.player.transform;
        _AudioSource = GetComponent<AudioSource>();

        if (_DefaultHolders.Length != _TopViewHolders.Length)
        {
            Debug.LogError("Top View holders must have the same length as default holders!");
            Debug.Break();
        }

        // Calculate the middle of the camera array, and access variables from the middle
        _HolderIndex = Mathf.FloorToInt(_DefaultHolders.Length / 2);
        _CurrentHolder = _DefaultHolders[_HolderIndex];
        _OrbitRadius = _CurrentHolder._Offset.x;
        _GroundOffset = _CurrentHolder._Offset.y;
    }

    void Update()
    {
        // Rotate the camera to look at the Player
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.LookRotation(_PlayerPosition.position - transform.position),
                                             _LookAtRotationSpeed * Time.deltaTime);

        ApplyCurrentHolder();
        HandleControls();
    }

    /// <summary>
    /// Applies the CurrentHolder variable to the Camera's variables
    /// </summary>
    void ApplyCurrentHolder()
    {
        // Calculate the offset from the ground using the players current position and our additional Y offset
        float groundOffset = _CurrentHolder._Offset.y + _PlayerPosition.position.y;
        // Store the orbit radius in case we need to alter it when moving onto a higher plane
        float orbitRadius = _CurrentHolder._Offset.x;
        if (Physics.SphereCast(transform.position + (Vector3.up * _EasingHeightOffset),
                               _HeightSphereRadius,
                               Vector3.down,
                               out RaycastHit hit,
                               _DistForHeightChange,
                               _MapLayer))
        {
            float offset = Mathf.Abs(_PlayerPosition.position.y - hit.point.y);
            groundOffset += offset;
            orbitRadius += offset / 1.5f;
        }

        // Smoothly change the OrbitRadius, GroundOffset and the Camera's field of view
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
        _OrbitRadius = Mathf.Lerp(_OrbitRadius, orbitRadius, _OrbitChangeSpeed * Time.deltaTime);
        _GroundOffset = Mathf.Lerp(_GroundOffset, groundOffset, _HeightChangeSpeed * Time.deltaTime);

        // Calculates the position the Camera wants to be in, using Ground Offset and Orbit Radius
        Vector3 targetPosition = (transform.position - _PlayerPosition.position).normalized
                                 * Mathf.Abs(_OrbitRadius)
                                 + _PlayerPosition.position;

        targetPosition.y = _GroundOffset;


        transform.position = Vector3.Lerp(transform.position, targetPosition, _FollowSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handles every type of control the Player has over the Camera
    /// </summary>
    void HandleControls()
    {
        // Check if we're holding either the Left or Right trigger and
        // rotate around the player using TriggerRotationSpeed if so
        if (Input.GetButton("RightTrigger"))
        {
            RotateView(-_TriggerRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetButton("LeftTrigger"))
        { 
            RotateView(_TriggerRotationSpeed * Time.deltaTime);
        }

        // As we've let go of the triggers, reset the desired new rotation
        if (Input.GetButtonUp("RightTrigger") || Input.GetButtonUp("LeftTrigger"))
        {
            _CurrentRotation = 0;
        }

        if (Input.GetButtonDown("ZoomLevel"))
        {
            _HolderIndex++;
            ApplyChangedZoomLevel(_TopView ? _TopViewHolders : _DefaultHolders);
        }
        if (Input.GetButtonDown("CameraAngle"))
        {
            _TopView = !_TopView; // Invert the TopView 
            ApplyChangedZoomLevel(_TopView ? _TopViewHolders : _DefaultHolders);
        }

        if (Input.GetButtonDown("CameraReset"))
        {
            StartCoroutine(ResetCamOverTime(_CameraResetLength));
        }
    }

    /// <summary>
    /// Rotates the camera using a given angle around the Player
    /// </summary>
    /// <param name="angle"></param>
    void RotateView(float angle)
    {
        _CurrentRotation = Mathf.Lerp(_CurrentRotation, angle, _RotateAroundSpeed * Time.deltaTime);
        transform.RotateAround(_PlayerPosition.position, Vector3.up, _CurrentRotation);
    }

    /// <summary>
    /// Changes zoom level based on the holder index, and plays audio
    /// </summary>
    /// <param name="currentHolder"></param>
    void ApplyChangedZoomLevel(CameraHolder[] currentHolder)
    {
        _AudioSource.PlayOneShot(_ChangeZoomAudio);

        if (_HolderIndex > currentHolder.Length - 1)
            _HolderIndex = 0;

        _CurrentHolder = currentHolder[_HolderIndex];
    }

    IEnumerator ResetCamOverTime(float length)
    {
        //TODO

        yield return null;
    }
}
