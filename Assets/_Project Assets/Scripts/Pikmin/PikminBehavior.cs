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
    public enum States { Idle, Formation, Attacking }

    [Header("Components")]
    public PikminSO _Data; // Only public for debugging reasons
    public States _State;

    Rigidbody _Rigidbody;
    Player _Player;

    void IPooledObject.OnObjectSpawn()
    {
        _Player = Player.player;
        _Rigidbody = GetComponent<Rigidbody>();

        // Set required variables
        _State = States.Idle;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1f) == false)
        {
            _Rigidbody.MovePosition(_Rigidbody.position + Vector3.down * _Data._Gravity * Time.fixedDeltaTime);
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

        var direction = _Player.transform.position - _Rigidbody.position;
        direction.y = 0;
        direction.Normalize();

        _Rigidbody.MovePosition(_Rigidbody.position + (direction * _Data._MovementSpeed * Time.fixedDeltaTime));
    }

    void HandleAttacking()
    {
        print("I be attackin'");
    }

    #region Setters
    public void SetData(PikminSO setTo) => _Data = setTo;
    #endregion
}
