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
    public enum States { Idle, Formation, Attacking }

    [Header("Components")]
    public PikminSO _Data; // Only public for debugging reasons
    public States _State;

    Vector3 _Velocity;
    Rigidbody _Rigidbody;
    Player _Player;

    void IPooledObject.OnObjectSpawn()
    {
        _Player = Player.player;
        _Rigidbody = GetComponent<Rigidbody>();

        _Velocity = Vector3.zero;
        _State = States.Idle;
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
            default:
                break;
        }
    }

    // Movement using rigidbody requires FixedUpdate
    void FixedUpdate()
    {
        switch (_State)
        {
            case States.Formation:
                HandleFormation();
                break;
            default:
                break;
        }
    }

    void HandleIdle()
    {
        print("I be idlin'");
    }

    void HandleFormation()
    {
        print("I be in formation!");
        MoveTowards(_Player.transform.position);
    }

    void HandleAttacking()
    {
        print("I be attackin'");
    }

    void MoveTowards(Vector3 towards)
    {
        // cache the direction of the player
        var direction = (towards - transform.position);

        // calculate the velocity needed
        var velocity = (direction.normalized) * _Data._MovementSpeed;
        velocity.y = _Rigidbody.velocity.y;
        _Rigidbody.velocity = velocity;

        direction.y = 0;
        // look at the direction of the player and smoothly interpolate to it
        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _Data._RotationSpeed * Time.deltaTime);
    }

    #region Setters
    public void SetData(PikminSO setTo) => _Data = setTo;
    #endregion
}
