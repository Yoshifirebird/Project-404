/*
 * Fun_GunController.cs
 * Created by: Ambrosia
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using System.Collections.Generic;
using UnityEngine;

public class Fun_GunController : MonoBehaviour {
  [Header ("Components")]
  [SerializeField] AudioClip _ShotSound = null;

  [Header ("Settings")]
  [SerializeField] float _MaxShootDistance = 50;
  [SerializeField] float _ShotDamage = 5;
  [SerializeField] float _BulletForce = 2.5f;
  [SerializeField] float _PikminRagdollTime = 2;

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
        hitInfo.rigidbody.AddForce (cursorRay.direction * _BulletForce);
      }

      if (cursorOnCollider && hitInfo.transform.CompareTag ("Pikmin")) {
        PikminAI aiComponent = hitInfo.collider.GetComponent<PikminAI> ();
        if (aiComponent != null && _ShotPikmin.Contains (aiComponent) == false) {
          // Damage the pikmin, which kills it
          aiComponent.Die(_PikminRagdollTime);

          // Add it to the list so we can't kill it after it's dead
          _ShotPikmin.Add (aiComponent);
          return;
        }
      }

      var health = GetComponent<IHealth> ();
      if (health != null) {
        health.SubtractHealth (_ShotDamage);
      }
    }

    transform.rotation = Quaternion.LookRotation (cursorRay.direction, _MainCamera.transform.up);
  }
}
