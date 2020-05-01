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
  [SerializeField] float _RotationSpeed = 5;
  [SerializeField] float _MovementSpeed = 5;

  [Header ("Camera Entries")]
  [SerializeField] CameraEntry[] _DefaultEntries = { new CameraEntry (5, 5, 50), new CameraEntry (6, 6, 60), new CameraEntry (7, 7, 70) };

  [Header("Debugging")]
  [SerializeField] float _RotationAngle = 0;

  int _EntryIndex = 0;
  CameraEntry _CurrentEntry = null;
  AudioSource _AudioSource = null;

  void Awake () {
    _AudioSource = GetComponent<AudioSource> ();

    _CurrentEntry = _DefaultEntries[0];
  }

  void Update () {
    if (Input.GetButtonDown ("Right Bumper")) {
      ChangeCameraZoom ();
    }
  }

  void ChangeCameraZoom () {
    // Increment and wrap the entry index
    _EntryIndex = (_EntryIndex > _DefaultEntries.Length) ? 0 : _EntryIndex + 1;
    _CurrentEntry = _DefaultEntries[_EntryIndex];
    _AudioSource.PlayOneShot (_ZoomSound);
  }

  void OnDrawGizmosSelected () {
    if (_DefaultEntries == null || _Target == null) {
      return;
    }

    foreach (var entry in _DefaultEntries) {
      Vector3 startingPosition = MathUtil._2Dto3D (MathUtil.PositionInUnit (_RotationAngle, entry._ForwardOffset), entry._HeightOffset);
      Gizmos.DrawCube(_Target.position + startingPosition, Vector3.one * 0.1f);
      Gizmos.DrawLine (_Target.position + startingPosition, _Target.position);
    }
  }
}