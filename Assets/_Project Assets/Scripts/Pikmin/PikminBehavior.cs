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
    // Enumerations
    public enum States { Idle, Formation, Attacking }
    public enum Colour { Red, Blue, Yellow }

    //[Header("Components")]
    Rigidbody _Rigidbody;
    Player _Player;

    [Header("Movement")]
    [SerializeField] float _MovementSpeed = 5f;
    [SerializeField] float _RotationSpeed = 5f;

    [Header("AI")]
    [SerializeField] float _StoppingDistance = 2f;
    [SerializeField] [Range(-1, 1)] float _StoppingAngle = 0.5f;

    [HideInInspector] public States _State;
    [HideInInspector] public Transform _TargetPosition;

    void IPooledObject.OnObjectSpawn()
    {
        // Grab components
        _Rigidbody = GetComponent<Rigidbody>();
        _Player = Player.player;

        // Set required variables
        _State = States.Idle;
    }

    void Update()
    {
        switch (_State)
        {
            case States.Idle:
                // Would play idle animation,
                // check surroundings for things to do
                break;
            case States.Formation:
                MoveTowards(_TargetPosition.position);
                break;
            case States.Attacking:
                break;
        }
    }

    void MoveTowards(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) <= _StoppingDistance)
            return;

        // Calculations to check whether or not the AI are in front or behind the leader
        var heading = position - transform.position;
        var dot = Vector3.Dot(heading, transform.forward);

        // Get the target direction we want to move in, set the Y velocity to 0
        // so we don't glide into the air, and move the AI
        Vector3 direction = position - transform.position;
        direction.y = 0;

        if (_StoppingAngle < dot)
        {
            // To-do: make the AI follow the player and attempt to keep behind him at all times
        }

        // Rotate the Pikmin to look towards where they're moving
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _RotationSpeed * Time.deltaTime);

        Vector3 velocity = direction.normalized * _MovementSpeed;
        velocity.y = _Rigidbody.velocity.y;
        _Rigidbody.velocity = velocity;
    }
}
