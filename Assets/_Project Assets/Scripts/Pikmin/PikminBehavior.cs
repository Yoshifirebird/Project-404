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
     * Carrying - latched onto another object and helping move it 
     * Dead - do a few things like destroying itself
     */
    public enum States { Idle, Formation, Attacking, Dead, Carrying, WaitingNull }

    [Header("Components")]
    public PikminSO _Data;
    States _State;
    States _PreviousState;

    float _AttackTimer = 0;
    GameObject _AttackingObject;

    Rigidbody _Rigidbody;
    Player _Player;
    Collider _Collider;
    PlayerPikminManager _PlayerPikminManager;

    void IPooledObject.OnObjectSpawn()
    {
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

        // Add ourself into the stats
        _PlayerPikminManager.AddPikminOnField(gameObject);
        PlayerStats.IncrementTotal(_Data._Colour);

        // Reset state machines
        _State = States.Idle;
        _PreviousState = States.Idle;

        // Reset state-specific variables
        _AttackingObject = null;
        _AttackTimer = 0;

        if (_Data._UsingTempModel)
        {
            // TODO: Figure out how to change colour of material
        }
    }

    void Update()
    {
        // Check if we've been attacking and we still have a valid attacking object
        if (_PreviousState == States.Attacking && _AttackingObject != null)
        {
            // Call the OnDetach function
            IPikminAttack aInterface = _AttackingObject.GetComponent<IPikminAttack>();

            if (aInterface != null) // Not quite sure why we need to do this
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

    void HandleFormation() => MoveTowards(_PlayerPikminManager.GetFormationCenter().position, GetSpeed(_Data._HeadType));

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

    #region General Purpose Useful Functions

    void MoveTowards(Vector3 towards, float speed)
    {
        // cache the direction of the player
        Vector3 direction = (towards - transform.position).normalized;

        // calculate the velocity needed
        Vector3 velocity = direction * speed;
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
