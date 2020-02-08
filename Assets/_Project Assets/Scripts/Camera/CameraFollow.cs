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
    [SerializeField] Transform _ToFollow;
    [SerializeField] CFCameraVars[] _DefaultHolders;    // Default View variables
    [SerializeField] CFCameraVars[] _TopViewHolders;  // Top View variables
    Camera _MainCamera;

    [Header("Audio")]
    [SerializeField] AudioClip _ChangeZoomAudio;
    AudioSource _AudioSource;

    [Header("Settings")]
    [SerializeField] float _FollowSpeed;
    [SerializeField] float _FOVChangeSpeed;
    [SerializeField] float _FocusRotationSpeed;
    [SerializeField] float _AdjustRotationSpeed;

    int _HolderIndex;
    CFCameraVars _CurrentHolder;
    float _OrbitRadius;
    float _GroundOffset;
    bool _TopView = false;

    CameraFollow()
    {
        // Apply movement/rotation settings
        _FollowSpeed = 5;
        _FocusRotationSpeed = 5;
        _FOVChangeSpeed = 2;
        _AdjustRotationSpeed = 2;
    }

    void Awake()
    {
        // Grab the main camera's 'Camera' component
        _MainCamera = Camera.main;
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
    }

    void LateUpdate()
    {
        // To stop jittery rotation, we apply the LookAt after 
        // rotation and movement has been applied
        transform.LookAt(_ToFollow.position);
    }

    void ApplyCurrentHolder()
    {
        // Set OrbitRadius and GroundOffset to the X and Y offsets of the current holder
        _OrbitRadius = Mathf.Lerp(_OrbitRadius, _CurrentHolder._Offset.x, _FOVChangeSpeed * Time.deltaTime);
        _GroundOffset = Mathf.Lerp(_GroundOffset, _CurrentHolder._Offset.y + _ToFollow.transform.position.y, _FOVChangeSpeed * Time.deltaTime);

        // Calculate the position we're moving to, apply the offset and then move the camera
        Vector3 targetPosition = (transform.position - _ToFollow.transform.position).normalized
                                 * Mathf.Abs(_OrbitRadius)
                                 + _ToFollow.transform.position;
        targetPosition.y = _GroundOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _FollowSpeed * Time.deltaTime);

        // Change the field of view to the currently selected FOV
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _CurrentHolder._FOV, _FOVChangeSpeed * Time.deltaTime);
    }

    void RotateView(float direction)
    {
        // Move the camera right by an offset of 'direction'
        // This works because we call 'LookAt' in LateUpdate
        transform.Translate(Vector3.right * Time.deltaTime * direction);
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
            RotateView(_AdjustRotationSpeed);
        else if (Input.GetButton("LeftTrigger"))
            RotateView(-_AdjustRotationSpeed);

        if (Input.GetButton("CameraReset"))
        {
            // Todo: make camera reset back to it's initial position behind the player
            Vector3 newPosition = _ToFollow.localPosition - _ToFollow.forward;
            transform.position = Vector3.Lerp(transform.position, newPosition + new Vector3(0, _CurrentHolder._Offset.y, _CurrentHolder._Offset.x), _FocusRotationSpeed * Time.deltaTime);
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
}
