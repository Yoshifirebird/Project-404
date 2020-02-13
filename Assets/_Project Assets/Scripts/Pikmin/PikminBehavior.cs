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
    public enum States { Idle, Formation, Attacking, Dead }

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
        var pikminManager = _Player.GetPikminManager();
        pikminManager.IncrementPikminOnField();
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

    void HandleIdle()
    {
        // stubbed
    }

    void HandleFormation()
    {
        MoveTowards(_PlayerPikminManager.GetFormationCenter().position, GetSpeed(_Data._HeadType));
    }

    void HandleAttacking()
    {
        // stubbed
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

    void MoveTowards(Vector3 towards, float speed)
    {
        // cache the direction of the player
        var direction = (towards - transform.position);

        // calculate the velocity needed
        var velocity = (direction.normalized) * speed;
        velocity.y = _Rigidbody.velocity.y;
        _Rigidbody.velocity = velocity;

        direction.y = 0;
        // look at the direction of the player and smoothly interpolate to it
        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _Data._RotationSpeed * Time.deltaTime);
    }

    float GetSpeed(Headtype headtype)
    {
        switch(headtype)
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
            _Player.GetPikminManager().IncrementSquadCount();
        }
    }

    public void RemoveFromSquad()
    {
        if (_State == States.Formation)
        {
            ChangeState(States.Idle);
            _Player.GetPikminManager().DecrementSquadCount();
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
}
