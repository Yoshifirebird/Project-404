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
    [SerializeField] CFCameraVars[] _DefaultHolders;    // Default View variables
    [SerializeField] CFCameraVars[] _TopViewHolders;  // Top View variables
    Camera _MainCamera;

    [Header("Audio")]
    [SerializeField] AudioClip _ChangeZoomAudio;
    AudioSource _AudioSource;

    [Header("Movement / Camera Specific")]
    [SerializeField] float _FollowSpeed;
    [SerializeField] float _FOVChangeSpeed;

    [Header("Rotation")]
    [SerializeField] float _RotateTowardsTargetSpeed;

    [SerializeField] float _CircleRotateSpeed;
    [SerializeField] float _ResetRotationSpeed;
    [SerializeField] float _QERotationSpeed;

    Transform _PlayerPosition;
    PlayerMovementController _MovementController;
    int _HolderIndex;
    CFCameraVars _CurrentHolder;
    float _OrbitRadius;
    float _GroundOffset;
    bool _TopView = false;

    CameraFollow()
    {
        // Apply movement/rotation settings
        _FollowSpeed = 5;
        _RotateTowardsTargetSpeed = 5;
        _ResetRotationSpeed = 5;
        _FOVChangeSpeed = 2;
        _QERotationSpeed = 2;
    }

    void Awake()
    {
        // Grab the main camera's 'Camera' component
        _MainCamera = Camera.main;
        _PlayerPosition = Player.player.transform;
        _MovementController = Player.player.GetMovementController();
        _AudioSource = GetComponent<AudioSource>();

        if (_DefaultHolders.Length != _TopViewHolders.Length)
        {
            Debug.LogError("Top View holders must have the same length as default holders!");
            Debug.Break();
        }

        // Assign the current holder and the associated variables
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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_PlayerPosition.position - transform.position), _RotateTowardsTargetSpeed * Time.deltaTime);
    }

    void ApplyCurrentHolder()
    {
        // Set OrbitRadius and GroundOffset to the X and Y offsets of the current holder
        _OrbitRadius = Mathf.Lerp(_OrbitRadius, _CurrentHolder._Offset.x, _FOVChangeSpeed * Time.deltaTime);
        _GroundOffset = Mathf.Lerp(_GroundOffset, _CurrentHolder._Offset.y + _PlayerPosition.transform.position.y, _FOVChangeSpeed * Time.deltaTime);

        // Calculate the position we're moving to, apply the offset and then move the camera
        Vector3 targetPosition = (transform.position - _PlayerPosition.transform.position).normalized
                                 * Mathf.Abs(_OrbitRadius)
                                 + _PlayerPosition.transform.position;
        targetPosition.y = _GroundOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _FollowSpeed * Time.deltaTime);

        // Change the field of view to the currently selected FOV
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
    }

    void RotateView(float direction)
    {
        // Move the camera right by an offset of 'direction'
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * direction, _CircleRotateSpeed * Time.deltaTime);
    }

    void ApplyChangedZoomLevel(CFCameraVars[] currentHolder)
    {
        // Play the zoom audio
        _AudioSource.PlayOneShot(_ChangeZoomAudio);

        // Wrap the holder index
        if (_HolderIndex > currentHolder.Length - 1)
            _HolderIndex = 0;

        // Assign the current holder
        _CurrentHolder = currentHolder[_HolderIndex];
    }

    void HandleControls()
    {
        if (Input.GetButton("RightTrigger"))
            RotateView(_QERotationSpeed);
        else if (Input.GetButton("LeftTrigger"))
            RotateView(-_QERotationSpeed);

        if (Input.GetButton("CameraReset"))
        {
            // Calculate the difference between the two rotations
            float rotateBy = transform.eulerAngles.y - _MovementController._RotationBeforeIdle.eulerAngles.y;
            // Make sure we don't rotate over 360 degrees, there is literally no point
            if (rotateBy > 180)
                rotateBy -= 360;
            else if (rotateBy < -180)
                rotateBy += 360;

            // Finally rotate using the focus speed
            RotateView(rotateBy * _ResetRotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("ZoomLevel"))
        {
            // Increment the holder index to go to the next zoom level
            _HolderIndex++;
            // Does a basic check to see if we're in top view, and if it is use the alternate holders
            ApplyChangedZoomLevel(_TopView ? _TopViewHolders : _DefaultHolders);
        }

        // Check if the player presses the CameraAngle button
        if (Input.GetButtonDown("CameraAngle"))
        {
            ApplyChangedZoomLevel(_TopView ? _DefaultHolders : _TopViewHolders);
            _TopView = !_TopView;
        }
    }
}
