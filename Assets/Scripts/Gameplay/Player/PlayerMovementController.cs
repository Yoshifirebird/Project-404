/*
 * PlayerMovementController.cs
 * Created by: Ambrosia
 * Created on: 1/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
  [Header ("Settings")]
  [SerializeField] float _MaxSpeed = 3.5f;
  [SerializeField] float _Acceleration = 7.5f;

  [SerializeField] float _JumpSpeed = 5;
  float _CurrentSpeed = 0;

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

    if (GameManager._FunMode && Input.GetKeyDown(KeyCode.Space))
    {
        _Rigidbody.velocity = new Vector3(_Rigidbody.velocity.x, _JumpSpeed, _Rigidbody.velocity.z);
      print("A");
    }

    Vector3 input = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

    if (input == Vector3.zero) {
      if (_CurrentSpeed != 0) {
        _CurrentSpeed = 0;
      }
      return;
    }

    Vector3 rotation = _MainCamera.transform.TransformDirection (input);
    rotation.y = 0;
    transform.rotation = Quaternion.LookRotation (rotation);

    // To prevent instant, janky movement we step towards the resultant max speed according to _Acceleration
    _CurrentSpeed = Mathf.SmoothStep (_CurrentSpeed, _MaxSpeed, _Acceleration * Time.deltaTime);

    Vector3 newVelocity = rotation.normalized * _CurrentSpeed;
    newVelocity.y = _Rigidbody.velocity.y;
    _Rigidbody.velocity = newVelocity;
  }
}
