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
  [Header ("Settings")]
  [SerializeField] float _RotationSpeed = 5;
  [SerializeField] float _MovementSpeed = 5;

  [Header ("Camera Entries")]
  [SerializeField] CameraEntry[] _DefaultEntries = { new CameraEntry(5, 5, 50), new CameraEntry(6, 6, 60), new CameraEntry(7, 7, 70) };
  [SerializeField] CameraEntry[] _TopEntries = { new CameraEntry(6, 5, 50), new CameraEntry(7, 6, 60), new CameraEntry(8, 7, 70) };
  CameraEntry _CurrentEntry = null;
}