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

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1) == false)
        {
            transform.position += Vector3.down * _Data._Gravity * Time.deltaTime;
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

        transform.position = Vector3.Lerp(transform.position, _Player.transform.position, _Data._MovementSpeed * Time.deltaTime);
    }

    void HandleAttacking()
    {
        print("I be attackin'");
    }

    /* void TurnRagdoll () => _Rigidbody.isKinematic = false;     */
    #region Setters
    public void SetData(PikminSO setTo) => _Data = setTo;
    #endregion
}
