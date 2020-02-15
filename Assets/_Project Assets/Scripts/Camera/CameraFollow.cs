/*
 * CameraFollow.cs
 * Created by: Neo
 * Created on: 6/2/2020 (dd/mm/yy)
 * Created for: following a target with incrementable offset and field of view
 */

using UnityEngine;

[System.Serializable]
public class CFCameraVars
{
    public float _FOV = 75f;
    public Vector2 _Offset = Vector2.one;
}

public class CameraFollow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CFCameraVars[] _DefaultHolders;
    [SerializeField] CFCameraVars[] _TopViewHolders;
    Camera _MainCamera;

    [Header("Audio")]
    [SerializeField] AudioClip _ChangeZoomAudio;

    [Header("Movement / Camera Specific")]
    [SerializeField] float _FollowSpeed;
    [SerializeField] float _FOVChangeSpeed;

    [Header("Rotation")]
    [SerializeField] float _LookAtRotationSpeed;

    [Header("Controlled Rotation")]
    [SerializeField] float _CameraResetSpeed;
    [SerializeField] float _TriggerRotationSpeed;

    CFCameraVars _CurrentHolder;
    AudioSource _AudioSource;
    Transform _PlayerPosition;
    PlayerMovementController _MovementController;
    float _OrbitRadius;
    float _GroundOffset;
    int _HolderIndex;
    bool _TopView = false;

    CameraFollow()
    {
        // Movement / Camera Specific
        _FollowSpeed = 5;
        _FOVChangeSpeed = 2;
        // Rotation
        _LookAtRotationSpeed = 5;
        // Controlled Rotation
        _CameraResetSpeed = 5;
        _TriggerRotationSpeed = 2;
    }

    void Awake()
    {
        _MainCamera = Camera.main;
        _PlayerPosition = Player.player.transform;
        _MovementController = Player.player.GetMovementController();
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
                                             0.15f);

        ApplyCurrentHolder();
        HandleControls();
    }

    /// <summary>
    /// Applies the CurrentHolder variable to the Camera's variables
    /// </summary>
    void ApplyCurrentHolder()
    {
        // Smoothly change the OrbitRadius, GroundOffset and the Camera's field of view
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
        _OrbitRadius = Mathf.Lerp(_OrbitRadius, _CurrentHolder._Offset.x, _FOVChangeSpeed * Time.deltaTime);
        _GroundOffset = Mathf.Lerp(_GroundOffset, _CurrentHolder._Offset.y + _PlayerPosition.transform.position.y, _FOVChangeSpeed * Time.deltaTime);

        // Calculates the position the Camera wants to be in, using Ground Offset and Orbit Radius
        Vector3 targetPosition = (transform.position - _PlayerPosition.transform.position).normalized
                                 * Mathf.Abs(_OrbitRadius)
                                 + _PlayerPosition.transform.position;
        targetPosition.y = _GroundOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.15f);
    }

    /// <summary>
    /// Handles every type of control the Player has over the Camera
    /// </summary>
    void HandleControls()
    {
        // Check if we're holding either the Left or Right trigger and
        // rotate around the player using TriggerRotationSpeed if so
        if (Input.GetButton("RightTrigger"))
            RotateView(_TriggerRotationSpeed);
        else if (Input.GetButton("LeftTrigger"))
            RotateView(-_TriggerRotationSpeed);

        if (Input.GetButtonDown("ZoomLevel"))
        {
            _HolderIndex++;
            ApplyChangedZoomLevel(_TopView ? _TopViewHolders : _DefaultHolders);
        }
        if (Input.GetButtonDown("CameraAngle"))
        {
            ApplyChangedZoomLevel(_TopView ? _DefaultHolders : _TopViewHolders);
            _TopView = !_TopView; // Invert if we're using the TopView
        }

        if (Input.GetButton("CameraReset"))
        {
            // Gets the difference between the two rotations, and then makes sure it doesn't overrotate
            float difference = transform.eulerAngles.y - _MovementController._RotationBeforeIdle.eulerAngles.y;
            if (difference > 180)
                difference -= 360;
            else if (difference < -180)
                difference += 360;
            // Invert the difference, convert it to radians and apply the camera reset speed
            RotateView(-difference * _CameraResetSpeed * Mathf.Deg2Rad);
        }
    }

    /// <summary>
    /// Rotates the camera using a given angle around the Player
    /// </summary>
    /// <param name="angle"></param>
    void RotateView(float angle) => transform.RotateAround(_PlayerPosition.position, Vector3.up, angle);

    /// <summary>
    /// Changes zoom level based on the holder index, and plays audio
    /// </summary>
    /// <param name="currentHolder"></param>
    void ApplyChangedZoomLevel(CFCameraVars[] currentHolder)
    {
        _AudioSource.PlayOneShot(_ChangeZoomAudio);

        if (_HolderIndex > currentHolder.Length - 1)
            _HolderIndex = 0;

        _CurrentHolder = currentHolder[_HolderIndex];
    }
}
