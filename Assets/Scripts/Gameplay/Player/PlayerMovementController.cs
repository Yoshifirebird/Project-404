/*
 * PlayerMovementController.cs
 * Created by: Ambrosia
 * Created on: 1/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
  [Header ("Settings")]
  [SerializeField] float _MovementSpeed = 2.5f;

  Vector3 worldVelocity;
  Vector3 lastPosition;

  Rigidbody _Rigidbody = null;
  Camera _MainCamera = null;

  void Awake () {
    _Rigidbody = GetComponent<Rigidbody> ();
    _MainCamera = Camera.main;
  }

  void FixedUpdate () {
    if (GameManager._IsPaused) {
      return;
    }

    //verlet integration
    worldVelocity = (_Rigidbody.position - lastPosition) / Time.fixedDeltaTime;
    lastPosition = _Rigidbody.position;

    //how much of our velocity is in line with the direction of gravity, ie 'how fast are we falling'
    Vector3 fallingVelocity = Vector3.Project (worldVelocity, Physics.gravity);

    Vector3 input = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

    Vector3 rotation = _MainCamera.transform.TransformDirection (input);
    rotation.y = 0;
    if (input != Vector3.zero) {
      transform.rotation = Quaternion.LookRotation (rotation);
    }

    Vector3 delta = rotation * _MovementSpeed;
    delta -= fallingVelocity + Physics.gravity * Time.fixedDeltaTime;
    _Rigidbody.velocity = delta;
  }
}