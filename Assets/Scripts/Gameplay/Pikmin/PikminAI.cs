/*
 * PikminAI.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum PikminStates
{
  Idle,
  RunningTowards,
  Attacking,
  Dead,
}

// Immediate states after running towards another object/position
public enum PikminIntention
{
  Attack, // TODO
  Carry, // TODO
  PullWeeds, // TODO
  Idle, // TODO (disbanding)
}

public class PikminAI : MonoBehaviour
{
  // Holds everything that makes a Pikmin unique
  [Header("Components")]
  public PikminObject _Data = null;
  [SerializeField] LayerMask _PikminInteractableMask = 0;

  [Header("VFX")]
  [SerializeField] GameObject _DeathParticle = null;

  [Header("Debugging")]
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
  [SerializeField] PikminMaturity _CurrentMaturity = default;
  [SerializeField] PikminStatSpecifier _CurrentStatSpecifier = default;
  [SerializeField] float _CurrentMoveSpeed = 0;
  [SerializeField] float _AttackTimer = 0;

  // Components
  AudioSource _AudioSource = null;
  Rigidbody _Rigidbody = null;
  Collider _Collider = null;

  #region Unity Methods
  void Awake()
  {
    _Rigidbody = GetComponent<Rigidbody>();
    _AudioSource = GetComponent<AudioSource>();
    _Collider = GetComponent<Collider>();

    _CurrentMaturity = _Data._StartingMaturity;

    _CurrentStatSpecifier = PikminStatSpecifier.OnField;
    PikminStatsManager.Add(_Data._Colour, _CurrentMaturity, _CurrentStatSpecifier);
  }

  void Update()
  {
    if (GameManager._IsPaused)
    {
      return;
    }

    switch (_CurrentState)
    {
      case PikminStates.Idle:
        HandleIdle();
        break;
      case PikminStates.RunningTowards:
        if (_TargetObject == null)
        {
          ChangeState(PikminStates.Idle);
        }
        else
        {
          MoveTowardsTarget();
        }
        break;
      case PikminStates.Attacking:
        HandleAttacking();
        break;
      case PikminStates.Dead:
        HandleDeath();
        break;
      default:
        break;
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (_TargetObjectCollider != null && collision.collider == _TargetObjectCollider)
    {
      CarryoutIntention();
    }
  }

  void OnCollisionExit(Collision collision)
  {
    if (collision.transform == _AttackingTransform && _CurrentState == PikminStates.Attacking)
    {
      // The object we've been attacking has just been destroyed, or smth else happened so we're gonna go idle
      if (_Attacking == null)
      {
        ChangeState(PikminStates.Idle);
        return;
      }

      _Attacking.OnAttackEnd(this);
      ChangeState(PikminStates.RunningTowards);

      _TargetObject = collision.transform;
      _TargetObjectCollider = collision.collider;
      _Intention = PikminIntention.Attack;
    }
  }

  #endregion

  #region States
  void CarryoutIntention()
  {
    // Run intention-specific logic (attack = OnAttackStart for the target object)
    switch (_Intention)
    {
      case PikminIntention.Attack:
        _AttackingTransform = _TargetObject;

        _Attacking = _TargetObject.GetComponent<IPikminAttack>();
        _Attacking.OnAttackStart(this);

        LatchOnto(_AttackingTransform);

        ChangeState(PikminStates.Attacking);
        break;
      case PikminIntention.Carry:
        break;
      case PikminIntention.PullWeeds:
        break;
      case PikminIntention.Idle:
        ChangeState(PikminStates.Idle);
        break;
      default:
        break;
    }

    _Intention = PikminIntention.Idle;
  }

  void HandleIdle()
  {
    // Look for a target object
    Collider[] objects = Physics.OverlapSphere(transform.position, _Data._SearchRadius, _PikminInteractableMask);
    foreach (Collider collider in objects)
    {
      // Check if the object can even be seen by the Pikmin
      Vector3 closestPointToPikmin = collider.ClosestPoint(transform.position);
      if (Physics.Raycast(transform.position, (closestPointToPikmin - transform.position).normalized, out RaycastHit hit, _Data._SearchRadius))
      {
        // See if the Collider we hit wasn't the Player OR the closest object, meaning we can't actually get to the object
        if (hit.collider != collider && hit.transform.CompareTag("Player") == false)
        {
          continue;
        }
      }

      // We can move to the target object, and it is an interactable, so set our target object
      ChangeState(PikminStates.RunningTowards);
      _TargetObject = collider.transform;
      _TargetObjectCollider = collider;
      _Intention = collider.GetComponent<IPikminInteractable>().IntentionType;
    }
  }

  void HandleDeath()
  {
    PikminStatsManager.Remove(_Data._Colour, _CurrentMaturity, _CurrentStatSpecifier);

    // Create the soul gameobject, and play the death noise
    Instantiate(_DeathParticle, transform.position, Quaternion.Euler(-90, 0, 0));
    AudioSource.PlayClipAtPoint(_Data._DeathNoise, transform.position, _Data._AudioVolume);
    // Remove the object
    Destroy(gameObject);
  }

  void HandleAttacking()
  {
    // The object we were attacking has died, so we can go back to being idle
    if (_AttackingTransform == null)
    {
      ChangeState(PikminStates.Idle);
      return;
    }

    // Add to the timer and attack if we've gone past the timer
    _AttackTimer += Time.deltaTime;
    if (_AttackTimer >= _Data._AttackDelay)
    {
      _Attacking.OnAttackRecieve(_Data._AttackDamage);
      _AttackTimer = 0;
    }
  }

  void ChangeState(PikminStates state)
  {
    _PreviousState = _CurrentState;
    _CurrentState = state;

    // Null out the variables we were using in the previous state
    if (_PreviousState == PikminStates.RunningTowards || _PreviousState == PikminStates.Idle && _TargetObject != null)
    {
      _TargetObject = null;
      _TargetObjectCollider = null;
    }
    else if (_PreviousState == PikminStates.Attacking)
    {
      LatchOnto(null);
      _Rigidbody.velocity = Vector3.zero;

      // Check if the object we were attacking was still active or not
      if (_AttackingTransform != null)
      {
        _Attacking = null;
        _AttackingTransform = null;
        return;
      }

      // As it is still active, and not null, we can call the appropriate function
      _Attacking.OnAttackEnd(this);
      _AttackingTransform = null;
      _AttackTimer = 0;
    }
  }
  #endregion

  void MoveTowardsTarget()
  {
    Vector3 closestPoint = ClosestPointOnTarget();

    // Rotate to look at the object we're moving towards
    Vector3 delta = (closestPoint - transform.position).normalized;
    delta.y = 0;
    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(delta), _Data._RotationSpeed * Time.deltaTime);

    // To prevent instant, janky movement we step towards the resultant max speed according to _Acceleration
    _CurrentMoveSpeed = Mathf.SmoothStep(_CurrentMoveSpeed, _Data._MaxMovementSpeed, _Data._AccelerationSpeed * Time.deltaTime);

    if (MathUtil.DistanceTo(transform.position, closestPoint) <= 5)
    {
      delta.y = closestPoint.y;
      if (Physics.Raycast(transform.position, delta, out RaycastHit hit, 2.5f))
      {
        if (hit.transform.CompareTag("Pikmin"))
        {
          if (Random.Range(0, 1) > 0.5f)
          {
            delta += transform.right * 2;
          }
          else
          {
            delta += -transform.right * 2;
          }
          delta += -transform.forward;
        }
      }
      delta.y = 0;
    }

    Vector3 newVelocity = delta.normalized * _CurrentMoveSpeed;
    newVelocity.y = _Rigidbody.velocity.y;
    _Rigidbody.velocity = newVelocity;
  }

  Vector3 ClosestPointOnTarget()
  {
    // Check if there is a collider for the target object we're running to
    if (_TargetObjectCollider != null)
    {
      // Our target is the closest point on the collider
      return _TargetObjectCollider.ClosestPointOnBounds(transform.position);
    }

    return _TargetObject.position;
  }

  void ChangePikminStat(PikminStatSpecifier newSpecifier)
  {
    PikminStatsManager.Remove(_Data._Colour, _CurrentMaturity, _CurrentStatSpecifier);
    PikminStatsManager.Add(_Data._Colour, _CurrentMaturity, newSpecifier);

    _CurrentStatSpecifier = newSpecifier;
  }

  public void StartRunTowards(Transform obj)
  {
    _TargetObject = obj;
    ChangeState(PikminStates.RunningTowards);
  }

  public void LatchOnto(Transform obj)
  {
    transform.parent = obj;
    _Rigidbody.isKinematic = (obj != null);
    _Collider.enabled = (obj == null);

    if (obj != null)
    {
      transform.position = obj.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
      transform.position -= transform.forward / 5;
    }
  }
}