/*
 * PikminBehavior.cs
 * Created by: Neo, Ambrosia
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: making the Pikmin have Artificial Intelligence
 */

/* Barebones basic Pikmin behaviour
 * Idle: look around, stay still, if an object it can interact with touches it, start Attacking with it
 * Formation: stay behind player, follow player if walks too far away, look at player
 * Attacking: attack object, check if still attacking (if not then go to idle)
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PikminBehavior : MonoBehaviour, IPooledObject
{
    /* Idle - do nothing as of (15/2/2020)
     * Formation - move towards formation position
     * Attacking - general term for attack, grab, etc.
     * Latched - holding onto another object
     * Dead - do a few things like destroying itself
     */
    public enum States { Idle, Formation, Attacking, Dead, WaitingNull }

    [Header("Components")]
    [SerializeField] PikminSO _Data;
    States _State;
    States _PreviousState;

    float _AttackTimer = 0;
    GameObject _AttackingObject;

    Rigidbody _Rigidbody;
    Player _Player;
    PlayerPikminManager _PlayerPikminManager;

    void IPooledObject.OnObjectSpawn()
    {
        if (_Player == null)
        {
            _Player = Player.player;
            _PlayerPikminManager = _Player.GetPikminManager();
        }

        if (_Rigidbody == null)
            _Rigidbody = GetComponent<Rigidbody>();

        // Add to the Pikmin on the field and the total amount of Pikmin
        _PlayerPikminManager.AddPikminOnField(gameObject);
        PlayerStats.IncrementTotal(_Data._Colour);

        // Reset state machines
        _State = States.Idle;
        _PreviousState = States.Idle;

        _AttackingObject = null;
    }

    void Update()
    {
        // Check if we've been attacking and we still have a valid attacking object
        if (_PreviousState == States.Attacking && _AttackingObject != null)
        {
            // null out the attacking object and reset the interaction timer
            _AttackingObject = null;
            _AttackTimer = 0;
        }

        switch (_State)
        {
            case States.Idle:
                HandleIdle();
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
    }

    void FixedUpdate()
    {
        switch (_State)
        {
            case States.Formation:
                // Movement using rigidbody requires FixedUpdate
                HandleFormation();
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If we're being thrown, or other
        if (_State == States.WaitingNull)
        {
            // See if there's anything beneath us
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                ChangeState(States.Idle);
                _Rigidbody.velocity = Vector3.zero;
            }

            // And check if we can attack
            CheckForAttack(collision.gameObject);
        }
    }

    void HandleIdle()
    {
        /* stubbed
        Set general regions for Pikmin to be dismissed into (not unlike formation center); likely
        dynamic regions based on the surrounding terrain (don't dismiss non-blue into water, etc.).
        If not dynamic, shift around which Pikmin are dismissed into which region.

        Simply move towards region. If interactable objects present (pellets, enemies, etc.),
        check for object's "territorial radius" and move towards to perform appropriate interaction
        (attack, carry, drink nectar, etc.).
        */
    }

    void HandleFormation() => MoveTowards(_PlayerPikminManager.GetFormationCenter().position, GetSpeed(_Data._HeadType));

    void HandleDeath()
    {
        // We may not have been properly removed from the squad, so do it ourself
        if (_PreviousState == States.Formation)
            RemoveFromSquad();

        PlayerStats.DecrementTotal(_Data._Colour);
        // TODO: handle death animation + timer later
        ObjectPooler.Instance.StoreInPool("Pikmin");
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
        _AttackingObject.GetComponent<IPikminAttack>().Attack(gameObject, _Data._AttackDamage);
        // Reset the timer as we've attacked
        _AttackTimer = 0;
    }

    void CheckForAttack(GameObject toCheck)
    {
        // Check if the object in question has the pikminattack component
        var interactable = toCheck.GetComponent<IPikminAttack>();
        if (interactable != null)
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
        GetComponent<Collider>().isTrigger = parent != null;
    }
    #endregion

    #region General Purpose Useful Functions

    void MoveTowards(Vector3 towards, float speed)
    {
        // cache the direction of the player
        Vector3 direction = (towards - transform.position);

        // calculate the velocity needed
        Vector3 velocity = direction.normalized * speed;
        velocity.y = _Rigidbody.velocity.y;
        _Rigidbody.velocity = velocity;

        direction.y = 0;
        // look at the direction of the player and smoothly interpolate to it
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _Data._RotationSpeed * Time.deltaTime);
    }

    float GetSpeed(Headtype headtype)
    {
        switch (headtype)
        {
            case Headtype.Bud:
                return 7;
            case Headtype.Flower:
                return 9;
            default:
                return 5;
        };
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
            _Player.GetPikminManager().AddToSquad(gameObject);
        }
    }

    public void RemoveFromSquad()
    {
        if (_State == States.Formation)
        {
            ChangeState(States.Idle);
            _Player.GetPikminManager().RemoveFromSquad(gameObject);
        }
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
