/*
 * CameraController.cs
 * Created by: Ambrosia
 * Created on: 1/5/2020 (dd/mm/yy)
 */

using UnityEngine;

[System.Serializable]
public class CameraEntry {
  public float _FOV = 60;
  public float _HeightOffset = 5;
  public float _ForwardOffset = 5;

  public CameraEntry (float height, float forward, float fov) {
    _HeightOffset = height;
    _ForwardOffset = forward;
    _FOV = fov;
  }
}

public class CameraController : MonoBehaviour {
  [Header ("Components")]
  [SerializeField] Transform _Target = null;
  [SerializeField] AudioClip _ZoomSound = null;

  [Header ("Settings")]
  [SerializeField] float _MoveSpeed = 5;

  [SerializeField] float _RotationDampening = 5;
  [SerializeField] float _RotationSpeed = 5;
  [SerializeField] float _RotationStepSpeed = 1;
  [SerializeField] float _FOVChangeSpeed = 5;

  [Header ("Camera Entries")]
  [SerializeField] CameraEntry[] _DefaultEntries = { new CameraEntry (5, 5, 50), new CameraEntry (6, 6, 60), new CameraEntry (7, 7, 70) };
  [SerializeField] CameraEntry[] _TopViewEntries = { new CameraEntry (5, 5, 50), new CameraEntry (6, 6, 60), new CameraEntry (7, 7, 70) };

  [Header ("Debugging")]
  [SerializeField] float _RotationAngle = 0;
  [SerializeField] float _WantedRotationAngle = 0;

  int _EntryIndex = 0;
  bool _TopView = false;
  CameraEntry _CurrentEntry = null;

  Vector3 _Velocity = Vector3.zero;

  Camera _MainCamera = null;
  AudioSource _AudioSource = null;

  void Awake () {
    _AudioSource = GetComponent<AudioSource> ();
    _MainCamera = Camera.main;

    _CurrentEntry = _DefaultEntries[0];
    // Reset position on runtime so we don't get a janky start to the game
    transform.position = GetWantedCameraPosition ();
  }

  void Update () {
    if (GameManager._IsPaused) {
      return;
    }

    if (Input.GetButtonDown ("Right Bumper")) {
      ChangeCameraZoom ();
    }
    else if (Input.GetButtonDown ("Left Bumper")) {
      _TopView = !_TopView;
      _CurrentEntry = _TopView ? _TopViewEntries[_EntryIndex] : _DefaultEntries[_EntryIndex];
    }

    if (Input.GetButton ("Left Trigger")) {
      _WantedRotationAngle += _RotationStepSpeed;
    }
    else if (Input.GetButton ("Right Trigger")) {
      _WantedRotationAngle -= _RotationStepSpeed;
    }

    if (Input.GetButton ("Right Stick Click")) {
      _WantedRotationAngle = -_Target.eulerAngles.y;
    }

    // Smoothly change values towards their intended target

    _RotationAngle = Mathf.Lerp (_RotationAngle, _WantedRotationAngle, _RotationSpeed * Time.deltaTime);
    _MainCamera.fieldOfView = Mathf.SmoothStep (_MainCamera.fieldOfView, _CurrentEntry._FOV, _FOVChangeSpeed * Time.deltaTime);

    transform.position = Vector3.Lerp (transform.position, GetWantedCameraPosition (), _MoveSpeed * Time.deltaTime);

    Vector3 delta = (_Target.position - transform.position).normalized;
    transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (delta), _RotationDampening * Time.deltaTime);
  }

  void ChangeCameraZoom () {
    // Increment and wrap the entry index
    _EntryIndex = (_EntryIndex + 1 >= _DefaultEntries.Length) ? 0 : _EntryIndex + 1;
    // Select the Current Entry based on the entry index
    _CurrentEntry = _TopView ? _TopViewEntries[_EntryIndex] : _DefaultEntries[_EntryIndex];
    // Play the zoom sound
    _AudioSource.PlayOneShot (_ZoomSound);
  }

  Vector3 GetWantedCameraPosition () {
    // Work out the Camera's 2D position around a unit circle, and then apply that offset to the target position to work out the wanted Position
    return _Target.position + MathUtil.XZToXYZ (MathUtil.PositionInUnit (_RotationAngle * Mathf.Deg2Rad, _CurrentEntry._ForwardOffset), _CurrentEntry._HeightOffset);
  }

  void OnDrawGizmosSelected () {
    if (_DefaultEntries == null || _Target == null) {
      return;
    }

    // Red cubes/lines = default view
    Gizmos.color = Color.red;
    foreach (var entry in _DefaultEntries) {
      Vector3 startingPosition = MathUtil.XZToXYZ (MathUtil.PositionInUnit (_RotationAngle * Mathf.Deg2Rad, entry._ForwardOffset), entry._HeightOffset);
      Gizmos.DrawCube (_Target.position + startingPosition, Vector3.one * 0.1f);
      Gizmos.DrawLine (_Target.position + startingPosition, _Target.position);
    }

    // Blue cubes/lines = top view
    Gizmos.color = Color.blue;
    foreach (var entry in _TopViewEntries) {
      Vector3 startingPosition = MathUtil.XZToXYZ (MathUtil.PositionInUnit (_RotationAngle * Mathf.Deg2Rad, entry._ForwardOffset), entry._HeightOffset);
      Gizmos.DrawCube (_Target.position + startingPosition, Vector3.one * 0.1f);
      Gizmos.DrawLine (_Target.position + startingPosition, _Target.position);
    }
  }
}
