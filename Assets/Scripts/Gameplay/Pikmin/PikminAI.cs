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

// Immediate states after running towards another object/position
public enum PikminIntention {
  Attack, // TODO
  Carry, // TODO
  PullWeeds, // TODO
  Idle, // TODO (disbanding)
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

  // Idle detection variables
  [SerializeField] PikminIntention _Intention = PikminIntention.Idle;
  [SerializeField] Transform _TargetObject = null;
  [SerializeField] Collider _TargetObjectCollider = null;

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
    if (GameManager._IsPaused) {
      // Disable Physics for the object
      if (_Rigidbody.isKinematic == false) {
        _Rigidbody.isKinematic = true;
      }

      return;
    }

    switch (_CurrentState) {
      case PikminStates.Idle:
        // Check if the target isn't null
        if (_TargetObject != null) {
          // Move towards the target we want to interact with
          MoveTowardsTarget ();

          // Check if we're within stopping distance of the object...
          float distanceToTarget = MathUtil.DistanceTo (transform.position, _TargetObject.position);
          if (distanceToTarget > _Data._StoppingDistance * _Data._StoppingDistance) {
            print ($"Beginning {_Intention.ToString()}!");

            // Run intention-specific logic (attack = OnAttackStart for the target object)
            switch (_Intention) {
              case PikminIntention.Attack:
                break;
              case PikminIntention.Carry:
                break;
              case PikminIntention.PullWeeds:
                break;
              case PikminIntention.Idle:
                ChangeState (PikminStates.Idle);
                break;
              default:
                break;
            }
          }

          break;
        }

        // Look for a target object
        Collider[] objects = Physics.OverlapSphere (transform.position, _Data._SearchRadius);
        foreach (Collider collider in objects) {
          // Check if the collider has the correct tab
          if (collider.CompareTag ("PikminInteractable") == false) {
            continue;
          }

          // Check if the object can even be seen by the Pikmin
          Vector3 closestPointToPikmin = collider.ClosestPoint (transform.position);
          if (Physics.Raycast (transform.position, (closestPointToPikmin - transform.position).normalized, out RaycastHit hit, _Data._SearchRadius)) {
            // See if the Collider we hit wasn't a Pikmin OR the Player OR the closest object, meaning we can't actually get to the object
            if (hit.collider != collider && hit.transform.CompareTag ("Pikmin") == false && hit.transform.CompareTag ("Player") == false) {
              continue;
            }
          }

          // We can move to the target object, and it is an interactable, so set our target object
          _TargetObject = collider.transform;
          _TargetObjectCollider = collider;
          _Intention = collider.GetComponent<IPikminInteractable> ().GetIntentionType ();
        }

        break;
        // RunningTowards is handled in FixedUpdate
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

    // Check if we're running towards the object, and 
    if (_CurrentState == PikminStates.RunningTowards) {
      MoveTowardsTarget ();
    }
  }

  void ChangeState (PikminStates state) {
    _PreviousState = _CurrentState;
    _CurrentState = state;

    // Null out the variables we were using in the previous state
    if (_PreviousState == PikminStates.RunningTowards && _TargetObject != null) {
      _TargetObject = null;
      _TargetObjectCollider = null;
    }
  }

  void MoveTowardsTarget () {
    Vector3 closestPoint = ClosestPointOnTarget ();

    // Rotate to look at the object we're moving towards
    Vector3 delta = (closestPoint - transform.position).normalized;
    delta.y = 0;
    transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (delta), _Data._RotationSpeed * Time.deltaTime);

    // Check if we're close enough already (stopping distance check)
    if (MathUtil.DistanceTo (transform.position, closestPoint) > _Data._StoppingDistance * _Data._StoppingDistance) {
      // Check if we're exceeding the max velocity, and move if not
      if (_Rigidbody.velocity.sqrMagnitude <= _Data._MaxMovementSpeed * _Data._MaxMovementSpeed) {
        _Rigidbody.AddRelativeForce (Vector3.forward * _Data._AccelerationSpeed);
      }
    }
  }

  Vector3 ClosestPointOnTarget () {
    // Check if there is a collider for the target object we're running to
    if (_TargetObjectCollider != null) {
      // Our target is the closest point on the collider
      return _TargetObjectCollider.ClosestPoint (transform.position);
    }

    return _TargetObject.position;
  }

  public void StartRunTowards (Transform obj) {
    _TargetObject = obj;
    ChangeState (PikminStates.RunningTowards);
  }
}