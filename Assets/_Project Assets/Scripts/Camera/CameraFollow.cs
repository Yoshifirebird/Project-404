/*
 * CameraFollow.cs
 * Created by: Neo, Ambrosia
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

[RequireComponent(typeof(AudioSource))]
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
    [SerializeField] float _HeightChangeSpeed;
    [SerializeField] float _OrbitChangeSpeed;
    [SerializeField] float _FOVChangeSpeed;

    [Header("Rotation")]
    [SerializeField] float _LookAtRotationSpeed;

    [Header("Controlled Rotation")]
    [SerializeField] float _CameraResetSpeed;
    [SerializeField] float _TriggerRotationSpeed;

    [Header("Miscellaneous")]
    [SerializeField] LayerMask _MapLayer;

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
        _HeightChangeSpeed = 2;
        _OrbitChangeSpeed = 2;
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
                                             _LookAtRotationSpeed * Time.deltaTime);

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

        float groundOffset = _CurrentHolder._Offset.y + _PlayerPosition.position.y;
        float orbitRadius = _CurrentHolder._Offset.x;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, _MapLayer))
        {
            float offset = Mathf.Abs(_PlayerPosition.position.y - hit.point.y);
            groundOffset += offset;
            orbitRadius += offset;
        }

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
            RotateView(-_TriggerRotationSpeed * Time.deltaTime);
        else if (Input.GetButton("LeftTrigger"))
            RotateView(_TriggerRotationSpeed * Time.deltaTime);

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

        if (Input.GetButton("CameraReset"))
        {
            // TODO: get audio for the camera reset and play it here

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
