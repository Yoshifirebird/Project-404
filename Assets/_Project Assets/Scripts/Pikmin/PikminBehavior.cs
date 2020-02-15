/*
 * PikminBehavior.cs
 * Created by: Neo, Ambrosia
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: making the Pikmin have Artificial Intelligence
 */

/* Barebones basic Pikmin behaviour
 * Idle: look around, stay still, if an object it can interact with touches it, start interacting with it
 * Formation: stay behind player, follow player if walks too far away, look at player
 * Attacking: attack object, check if still attacking (if not then go to idle)
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PikminBehavior : MonoBehaviour, IPooledObject
{
    public enum States { Idle, Formation, Attacking, Dead, Thrown }

    [Header("Components")]
    [SerializeField] PikminSO _Data;
    States _State;
    States _PreviousState;

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
        PlayerStats._TotalPikmin++;

        // Reset state machines
        _State = States.Idle;
        _PreviousState = States.Idle;
    }

    void Update()
    { 
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
            case States.Thrown:
                HandleThrown();
                break;
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
        if (_State == States.Thrown)
        {
            // See if there's anything beneath us
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                _State = States.Idle;
                _Rigidbody.velocity = Vector3.zero;
                // CHECK BENEATH US, TODO
            }
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

    void HandleFormation()
    {
        MoveTowards(_PlayerPikminManager.GetFormationCenter().position, GetSpeed(_Data._HeadType));
    }

    void HandleAttacking()
    {
        // stubbed

        /*
        Psuedo-code:
            if(Idle && InEnemyRadius){
                MoveTowards(Enemy);
                //Similar code to below, position and collision stuff different to account for ground-based
                //attack, etc.
            }

            if(Thrown){
                DetectEnemyCollision;
                Position = PointOnEnemyBody;
                DecreaseEnemyHealth;
                //Function involving enemy DEF, Pikmin ATK (weight*type?), and time (most basic function layout)
                //Deplete health every dt seconds, amount calculated by above values.
                //Maybe lock on if very close to enemy collider (similar to purples when stunning in 2).
            }
         */
    }

    void HandleDeath()
    {
        // We may not have been properly removed from the squad, so do it ourself
        if (_PreviousState == States.Formation)
            RemoveFromSquad();

        PlayerStats._TotalPikmin--;
        // TODO: handle death animation + timer later
        ObjectPooler.Instance.StoreInPool("Pikmin");
    }

    void HandleThrown()
    {
        /*
        Need to get whistle position. Calculate using trajectory formulas (get angle via atan).
        Create triangle based on x and z coords (abs of distance x from player to whistle cursor),
        generate some arbitrary height value y based on x, and calculate atan(y/x). 
        */

    }

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

    public void AddToSquad()
    {
        if (_State != States.Formation)
        {
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

    #region Setters
    public void SetData(PikminSO setTo) => _Data = setTo;

    public void ChangeState(States setTo)
    {
        _PreviousState = _State;
        _State = setTo;
    }
    #endregion

    #region Getters
    public States GetState() => _State;
    #endregion
}
