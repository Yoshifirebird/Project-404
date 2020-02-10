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
    AudioSource _AudioSource;

    [Header("Movement / Camera Specific")]
    [SerializeField] float _FollowSpeed;
    [SerializeField] float _FOVChangeSpeed;

    [Header("Rotation")]
    [SerializeField] float _LookAtTargetSpeed;
    [SerializeField] float _RotationCircleSpeed;
    [SerializeField] float _ResetRotationSpeed;
    [SerializeField] float _TriggerRotationSpeed;

    Transform _PlayerPosition;
    PlayerMovementController _MovementController;
    int _HolderIndex;
    CFCameraVars _CurrentHolder;
    float _OrbitRadius;
    float _GroundOffset;
    bool _TopView = false;

    CameraFollow()
    {
        _FollowSpeed = 5;
        _FOVChangeSpeed = 2;

        _LookAtTargetSpeed = 5;
        _RotationCircleSpeed = 7.5f;
        _ResetRotationSpeed = 5;
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

        _HolderIndex = Mathf.FloorToInt(_DefaultHolders.Length / 2);
        _CurrentHolder = _DefaultHolders[_HolderIndex];
        _OrbitRadius = _CurrentHolder._Offset.x;
        _GroundOffset = _CurrentHolder._Offset.y;
    }

    void Update()
    {
        ApplyCurrentHolder();
        HandleControls();

        // Rotate the camera
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.LookRotation(_PlayerPosition.position - transform.position),
                                             _LookAtTargetSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// Applies the CurrentHolder variable to the Camera's variables
    /// </summary>
    void ApplyCurrentHolder()
    {
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
        _OrbitRadius = Mathf.Lerp(_OrbitRadius, _CurrentHolder._Offset.x, _FOVChangeSpeed * Time.deltaTime);
        _GroundOffset = Mathf.Lerp(_GroundOffset, _CurrentHolder._Offset.y + _PlayerPosition.transform.position.y, _FOVChangeSpeed * Time.deltaTime);

        // Calculates the position the Camera wants to be in, using Ground Offset and Orbit Radius
        Vector3 targetPosition = (transform.position - _PlayerPosition.transform.position).normalized
                                 * Mathf.Abs(_OrbitRadius)
                                 + _PlayerPosition.transform.position;
        targetPosition.y = _GroundOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _FollowSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handles every type of control the Player has over the Camera
    /// </summary>
    void HandleControls()
    {
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
            _TopView = !_TopView;
        }

        if (Input.GetButton("CameraReset"))
        {
            // Gets the difference between the two rotations, and then makes sure it doesn't overrotate
            float difference = transform.eulerAngles.y - _MovementController._RotationBeforeIdle.eulerAngles.y;
            if (difference > 180)
                difference -= 360;
            else if (difference < -180)
                difference += 360;

            RotateView(difference * _ResetRotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Moves Camera in a given direction
    /// </summary>
    /// <param name="direction"></param>
    void RotateView(float direction)
    {
        transform.position = Vector3.Lerp(transform.position,
                                          transform.position + transform.right * direction,
                                          _RotationCircleSpeed * Time.deltaTime);
    }

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
