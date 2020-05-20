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
  [SerializeField] AudioClip _ShotSound = null;

  [Header ("Debugging")]
  [SerializeField] List<PikminAI> _ShotPikmin = new List<PikminAI> ();
  [SerializeField] AudioSource _AudioSource = null;
  [SerializeField] Camera _MainCamera = null;

  void Awake () {
    _MainCamera = Camera.main;
    _AudioSource = GetComponent<AudioSource> ();
  }

  void Update () {
    Ray cursorRay = _MainCamera.ScreenPointToRay (Input.mousePosition);
    bool cursorOnCollider = Physics.Raycast (cursorRay, out RaycastHit hitInfo, _MaxShootDistance);

    if (Input.GetButtonDown ("A Button")) {
      _AudioSource.PlayOneShot (_ShotSound);

      // Add force for extra fun points
      if (hitInfo.rigidbody != null) {
        hitInfo.rigidbody.AddForce (cursorRay.direction * _ShootForce);
      }

      if (cursorOnCollider && hitInfo.transform.CompareTag ("Pikmin")) {
        PikminAI aiComponent = hitInfo.collider.GetComponent<PikminAI> ();
        if (aiComponent != null) {
          aiComponent.GetComponent<IHealth> ().SubtractHealth (1);
        }
      }
    }

    transform.rotation = Quaternion.LookRotation (cursorRay.direction, _MainCamera.transform.up);
  }
}
