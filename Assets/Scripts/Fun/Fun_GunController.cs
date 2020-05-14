/*
 * Fun_GunController.cs
 * Created by: Ambrosia
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using System.Collections.Generic;
using UnityEngine;

public class Fun_GunController : MonoBehaviour {
  [Header ("Gun Settings")]
  [SerializeField] float _MaxShootDistance = 50;
  [SerializeField] float _ShootForce = 2.5f;

  [Header ("Misc")]
  [SerializeField] LayerMask _PikminLayer = 0;

  List<PikminAI> _ShotPikmin = new List<PikminAI> ();

  AudioSource _AudioSource = null;
  Camera _MainCamera = null;

  void Awake () {
    _MainCamera = Camera.main;
  }

  void Update () {
    Ray cursorRay = _MainCamera.ScreenPointToRay (Input.mousePosition);
    if (Physics.Raycast (cursorRay, out RaycastHit hitInfo, _MaxShootDistance, _PikminLayer) &&
      Input.GetButtonDown ("A Button")) {
      if (_AudioSource != null) {
        _AudioSource.Play ();
      }

      PikminAI aiComponent = hitInfo.collider.GetComponent<PikminAI> ();
      if (_ShotPikmin.Contains (aiComponent) == false) {
        hitInfo.collider.GetComponent<PikminAI> ().Fun_DIE ();
        hitInfo.collider.GetComponent<Rigidbody> ().AddForce (cursorRay.direction * _ShootForce);
        _ShotPikmin.Add (aiComponent);
      }
    }

    transform.rotation = Quaternion.LookRotation (cursorRay.direction, Vector3.up);
  }
}
