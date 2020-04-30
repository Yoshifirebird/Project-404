/*
 * PikminAI.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum PikminStates {
  Idle,
  RunningTowards,
}

public class PikminAI : MonoBehaviour {
  // Holds everything that makes a Pikmin unique
  [Header ("Components")]
  public PikminObject _Data = null;

  [Header ("Debugging")]
  // PreviousState is used to null any variables from the state it just changed from
  [SerializeField] PikminStates _CurrentState = PikminStates.Idle;
  [SerializeField] PikminStates _PreviousState = PikminStates.Idle;
  // RunningTowards
  [SerializeField] Transform _TargetObject = null;
  Rigidbody _Rigidbody = null;

  void Awake () {
    _Rigidbody = GetComponent<Rigidbody> ();
  }

  void Update () {
    switch (_CurrentState) {
      case PikminStates.Idle:
        break;
      case PikminStates.RunningTowards:
        Vector3 delta = (_TargetObject.position - transform.position).normalized;
        delta.y = 0;
        transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (delta), _Data._RotationSpeed * Time.deltaTime);
        break;
      default:
        break;
    }
  }

  void FixedUpdate () {
    float distanceTo = MathUtil.DistanceTo (transform.position, _TargetObject.position);
    print (distanceTo);
    if (_CurrentState == PikminStates.RunningTowards && distanceTo > _Data._StoppingDistance * _Data._StoppingDistance) {
      if (_Rigidbody.velocity.sqrMagnitude <= _Data._MaxMovementSpeed * _Data._MaxMovementSpeed) {
        _Rigidbody.AddRelativeForce (Vector3.forward * _Data._AccelerationSpeed);
      }
    }
  }

  void ChangeState (PikminStates state) {
    _PreviousState = _CurrentState;
    _CurrentState = state;

    if (_PreviousState == PikminStates.Idle && _TargetObject != null) {
      _TargetObject = null;
    }
  }

  public void StartRunTowards (Transform obj) {
    _TargetObject = obj;
    ChangeState (PikminStates.Idle);
  }
}