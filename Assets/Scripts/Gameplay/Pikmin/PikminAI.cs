/*
 * PikminAI.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum PikminStates {
  Idle,

  RunningTowards,

  Dead,
}

public class PikminAI : MonoBehaviour {
  // Holds everything that makes a Pikmin unique
  [Header ("Components")]
  public PikminObject _Data = null;
  public GameObject _DeathParticle = null;

  [Header ("Debugging")]
  // PreviousState is used to null any variables from the state it just changed from
  [SerializeField] PikminStates _CurrentState = PikminStates.Idle;
  [SerializeField] PikminStates _PreviousState = PikminStates.Idle;

  // RunningTowards
  [SerializeField] Transform _TargetObject = null;

  // Local stats
  PikminMaturity _CurrentMaturity = default;

  // Components
  AudioSource _AudioSource = null;
  Rigidbody _Rigidbody = null;

  void Awake () {
    _Rigidbody = GetComponent<Rigidbody> ();
    _AudioSource = GetComponent<AudioSource> ();

    _CurrentMaturity = _Data._StartingMaturity;

    PikminStatsManager.Add (_Data._Colour, _CurrentMaturity, PikminStatSpecifier.OnField);
  }

  void Update () {
    if (GameManager._IsPaused)
      return;

    switch (_CurrentState) {
      case PikminStates.Idle:
        break;
      case PikminStates.RunningTowards:
        Vector3 delta = (_TargetObject.position - transform.position).normalized;
        delta.y = 0;
        transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (delta), _Data._RotationSpeed * Time.deltaTime);
        break;
      case PikminStates.Dead:
        Instantiate (_DeathParticle, transform.position, Quaternion.Euler (-90, 0, 0));
        AudioSource.PlayClipAtPoint (_Data._DeathNoise, transform.position, _Data._AudioVolume);
        Destroy (gameObject);
        break;
      default:
        break;
    }
  }

  void FixedUpdate () {
    if (GameManager._IsPaused)
      return;

    float distanceToTarget = MathUtil.DistanceTo (transform.position, _TargetObject.position);
    if (_CurrentState == PikminStates.RunningTowards && distanceToTarget > _Data._StoppingDistance * _Data._StoppingDistance) {
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
    ChangeState (PikminStates.RunningTowards);
  }

}