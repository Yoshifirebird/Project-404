/*
 * PikminAI.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum PikminStates {
  Idle,
  RunningTowards,
  Attacking,
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
  [SerializeField] LayerMask _PikminInteractableMask = 0;

  [Header ("VFX")]
  [SerializeField] GameObject _DeathParticle = null;

  [Header ("Debugging")]
  // PreviousState is used to null any variables from the state it just changed from
  [SerializeField] PikminStates _CurrentState = PikminStates.Idle;
  [SerializeField] PikminStates _PreviousState = PikminStates.Idle;

  // Idle detection variables
  [SerializeField] PikminIntention _Intention = PikminIntention.Idle;
  [SerializeField] Transform _TargetObject = null;
  [SerializeField] Collider _TargetObjectCollider = null;

  // Attacking variables
  [SerializeField] IPikminAttack _Attacking = null;
  [SerializeField] Transform _AttackingTransform = null;

  // Local stats
  PikminMaturity _CurrentMaturity = default;
  float _AttackTimer = 0;

  // Components
  AudioSource _AudioSource = null;
  Rigidbody _Rigidbody = null;

  #region Unity Methods
  void Awake () {
    _Rigidbody = GetComponent<Rigidbody> ();
    _AudioSource = GetComponent<AudioSource> ();

    _CurrentMaturity = _Data._StartingMaturity;

    PikminStatsManager.Add (_Data._Colour, _CurrentMaturity, PikminStatSpecifier.OnField);
  }

  void Update () {
    if (GameManager._IsPaused) {
      // Disable Physics for the object and exit the function early
      if (_Rigidbody.isKinematic == false) {
        _Rigidbody.isKinematic = true;
      }
      return;
    }

    switch (_CurrentState) {
      case PikminStates.Idle:
        HandleIdle ();
        break;
        // RunningTowards is handled in FixedUpdate
      case PikminStates.Attacking:
        HandleAttacking ();
        break;
      case PikminStates.Dead:
        HandleDeath ();
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
      MoveTowardsTarget (_Data._StoppingDistance);
    }
  }
  #endregion

  #region States
  void HandleIdle () {
    // Check if the target isn't null
    if (_TargetObject != null) {
      // Move towards the target we want to interact with
      MoveTowardsTarget (_Data._IdleStoppingDistance);

      // Check if we're within stopping distance of the object...
      if (MathUtil.DistanceTo (transform.position, _TargetObject.position) <= _Data._InteractDistance * _Data._InteractDistance) {
        // Run intention-specific logic (attack = OnAttackStart for the target object)
        switch (_Intention) {
          case PikminIntention.Attack:
            _AttackingTransform = _TargetObject;

            _Attacking = _TargetObject.GetComponent<IPikminAttack> ();
            _Attacking.OnAttackStart (this);

            ChangeState (PikminStates.Attacking);
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

      return;
    }

    // Look for a target object
    Collider[] objects = Physics.OverlapSphere (transform.position, _Data._SearchRadius, _PikminInteractableMask);
    foreach (Collider collider in objects) {
      // Check if the object can even be seen by the Pikmin
      Vector3 closestPointToPikmin = collider.ClosestPoint (transform.position);
      if (Physics.Raycast (transform.position, (closestPointToPikmin - transform.position).normalized, out RaycastHit hit, _Data._SearchRadius)) {
        // See if the Collider we hit wasn't the Player OR the closest object, meaning we can't actually get to the object
        if (hit.collider != collider && hit.transform.CompareTag ("Player") == false) {
          continue;
        }
      }

      // We can move to the target object, and it is an interactable, so set our target object
      _TargetObject = collider.transform;
      _TargetObjectCollider = collider;
      _Intention = collider.GetComponent<IPikminInteractable> ().IntentionType;
    }
  }

  void HandleDeath () {
    Instantiate (_DeathParticle, transform.position, Quaternion.Euler (-90, 0, 0));
    AudioSource.PlayClipAtPoint (_Data._DeathNoise, transform.position, _Data._AudioVolume);
    Destroy (gameObject);
  }

  void HandleAttacking () {
    // The object we were attacking has died, so we can go back to being idle
    if (_Attacking == null) {
      ChangeState (PikminStates.Idle);
    }

    // Add to the timer and attack if we've gone past the timer
    _AttackTimer += Time.deltaTime;
    if (_AttackTimer >= _Data._AttackDelay) {
      _Attacking.OnAttackRecieve (_Data._AttackDamage);
      _AttackTimer = 0;
    }
  }

  void ChangeState (PikminStates state) {
    _PreviousState = _CurrentState;
    _CurrentState = state;

    // Null out the variables we were using in the previous state
    if (_PreviousState == PikminStates.RunningTowards || _PreviousState == PikminStates.Idle && _TargetObject != null) {
      _TargetObject = null;
      _TargetObjectCollider = null;
    } else if (_PreviousState == PikminStates.Attacking) {
      // Check if the object we were attacking was still active or not
      if (_AttackingTransform != null) {
        _Attacking = null;
        _AttackingTransform = null;
        return;
      }

      // As it is still active, and not null, we can call the appropriate function
      _Attacking.OnAttackEnd (this);
      _AttackingTransform = null;
      _AttackTimer = 0;
    }
  }
  #endregion

  void MoveTowardsTarget (float stoppingDistance) {
    Vector3 closestPoint = ClosestPointOnTarget ();

    // Rotate to look at the object we're moving towards
    Vector3 delta = (closestPoint - transform.position).normalized;
    delta.y = 0;
    transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (delta), _Data._RotationSpeed * Time.deltaTime);

    // Check if we're close enough already (stopping distance check)
    if (MathUtil.DistanceTo (transform.position, closestPoint) > stoppingDistance * stoppingDistance) {
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