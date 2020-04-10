/*
 * PikminBehavior.cs
 * Created by: Neo, Ambrosia
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: making the Pikmin have Artificial Intelligence
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PikminBehavior : MonoBehaviour, IPooledObject
{
    public enum States { Idle, Formation, Attacking, Dead, Carrying, Held, WaitingNull }
    States _State;
    States _PreviousState;

    [Header("Components")]
    public PikminSO _Data;

    Player _Player;
    PlayerPikminManager _PlayerPikminManager;
    Rigidbody _Rigidbody;
    Collider _Collider;
    Animator _Animator;

    float _AttackTimer = 0;
    GameObject _AttackingObject;

    bool _Spawned = false;

    void Spawn()
    {
        if (_Spawned)
            return;
        _Spawned = true;

        // Assign required variables if needed
        if (_Player == null)
        {
            _Player = Player.player;
            _PlayerPikminManager = _Player.GetPikminManager();
        }

        if (_Collider == null)
            _Collider = GetComponent<Collider>();

        if (_Rigidbody == null)
            _Rigidbody = GetComponent<Rigidbody>();

        if (_Animator == null)
            _Animator = GetComponent<Animator>();

        // Add ourself into the stats
        _PlayerPikminManager.AddPikminOnField(gameObject);
        PlayerStats.IncrementTotal(_Data._Colour);

        // Reset state machines
        _State = States.Idle;
        _PreviousState = States.Idle;

        // Reset state-specific variables
        _AttackingObject = null;
        _AttackTimer = 0;
    }

    void Start()
    {
        Spawn();
    }

    void IPooledObject.OnObjectSpawn()
    {
        Spawn();
    }

    void Update()
    {
        // Check if we've been attacking and we still have a valid attacking object
        if (_PreviousState == States.Attacking && _AttackingObject != null)
        {
            // Call the OnDetach function
            IPikminAttack aInterface = _AttackingObject.GetComponent<IPikminAttack>();

            if (aInterface != null)
                aInterface.OnDetach(gameObject);

            // Remove the attacking object and reset the timer
            _AttackingObject = null;
            _AttackTimer = 0;
        }

        switch (_State)
        {
            case States.Idle:
                HandleIdle();
                break;
            case States.Formation:
                HandleFormation();
                break;
            case States.Attacking:
                HandleAttacking();
                break;
            case States.Dead:
                HandleDeath();
                break;
            case States.WaitingNull:
            default:
                break;
        }
        HandleAnimation();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_State == States.WaitingNull)
        {
            CheckForAttack(collision.gameObject);

            if (_State != States.Attacking)
                ChangeState(States.Idle);
        }
    }

    void HandleAnimation()
    {

        Vector2 horizonalVelocity = new Vector2(_Rigidbody.velocity.x, _Rigidbody.velocity.z);
        if (horizonalVelocity.magnitude >= 3 && _Animator.GetBool("Walking") == false)
        {
            _Animator.SetBool("Walking", true);
        }
        else
        {
            _Animator.SetBool("Walking", false);
        }
    }

    void HandleIdle()
    {
        /*
        Set general regions for Pikmin to be dismissed into (not unlike formation center); likely
        dynamic regions based on the surrounding terrain (don't dismiss non-blue into water, etc.).
        If not dynamic, shift around which Pikmin are dismissed into which region.

        Simply move towards region. If interactable objects present (pellets, enemies, etc.),
        check for object's "territorial radius" and move towards to perform appropriate interaction
        (attack, carry, drink nectar, etc.).
        */
    }

    void HandleFormation() => MoveTowards(_PlayerPikminManager.GetFormationCenter().position);

    void HandleDeath()
    {
        // We may not have been properly removed from the squad, so do it ourself
        if (_PreviousState == States.Formation || _State == States.Formation)
        {
            RemoveFromSquad();
        }

        _PlayerPikminManager.RemovePikminOnField(gameObject);
        PlayerStats.DecrementTotal(_Data._Colour);
        // TODO: handle death animation + timer later
        //ObjectPooler.Instance.StoreInPool("Pikmin");
        Destroy(gameObject);
    }

    #region Attacking
    void HandleAttacking()
    {
        // If the thing we were Attacking with doesn't exist anymore
        if (_AttackingObject == null)
        {
            // Remove ourself from the attached object
            LatchOntoObject(null);
            ChangeState(States.Idle);
            return;
        }

        // Increment the timer to check if we can attack again
        _AttackTimer += Time.deltaTime;
        if (_AttackTimer < _Data._TimeBetweenAttacks)
            return;

        // We can attack, so grab the PikminAttack component and attack!
        IPikminAttack attackComponent = _AttackingObject.GetComponentInParent<IPikminAttack>();
        attackComponent.Attack(gameObject, _Data._AttackDamage);
        // Reset the timer as we've attacked
        _AttackTimer = 0;
    }

    void CheckForAttack(GameObject toCheck)
    {
        // Check if the object in question has the pikminattack component
        IPikminAttack interactable = toCheck.GetComponentInParent<IPikminAttack>();
        PikminBehavior pikmin = toCheck.GetComponent<PikminBehavior>();
        if (interactable != null && pikmin == null)
        {
            // It does, we can attack!
            // Set our state to attacking, assign the attack variables and latch!
            ChangeState(States.Attacking);
            _AttackingObject = toCheck;
            LatchOntoObject(toCheck.transform);
            interactable.OnAttach(gameObject);
        }
    }

    public void LatchOntoObject(Transform parent)
    {
        transform.parent = parent;
        _Rigidbody.isKinematic = parent != null;
    }
    #endregion

    public void EnterWater()
    {
        if (_Data._Colour != Colour.Blue)
        {
            // TODO: add struggling to get out of water
            ChangeState(States.Dead);
        }
    }

    // Gets called when the Player grabs the Pikmin
    public void ThrowHoldStart()
    {
        _Rigidbody.isKinematic = true;
        _Rigidbody.useGravity = false;
        _Collider.enabled = false;

        ChangeState(States.Held);
    }

    // Gets called when the Player releases the Pikmin
    public void ThrowHoldEnd()
    {
        _Rigidbody.isKinematic = false;
        _Rigidbody.useGravity = true;
        _Collider.enabled = true;

        RemoveFromSquad();
        ChangeState(States.WaitingNull);
    }

    #region General Purpose Useful Functions

    void MoveTowards(Vector3 towards)
    {
        Vector3 direction = (towards - _Rigidbody.position).normalized * _Data._MovementSpeed;
        direction.y = _Rigidbody.velocity.y;
        _Rigidbody.velocity = direction;

        // reset the Y axis so the body doesn't rotate up or down
        direction.y = 0;
        // look at the direction of the object we're moving towards and smoothly interpolate to it
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), _Data._RotationSpeed * Time.deltaTime);
    }

    #region Squad
    public void AddToSquad()
    {
        if (_State != States.Formation && _State != States.WaitingNull)
        {
            if (_AttackingObject != null)
            {
                LatchOntoObject(null);
            }

            ChangeState(States.Formation);
            _PlayerPikminManager.AddToSquad(gameObject);
        }
    }

    public void RemoveFromSquad()
    {
        ChangeState(States.Idle);
        _PlayerPikminManager.RemoveFromSquad(gameObject);
    }
    #endregion

    #region Setters
    public void SetData(PikminSO setTo) => _Data = setTo;

    public void ChangeState(States setTo)
    {
        _PreviousState = _State;
        _State = setTo;

        if (transform.parent != null)
            transform.parent = null;
    }
    #endregion

    #region Getters
    public States GetState() => _State;
    #endregion
    #endregion
}
