/*
 * PikminBehavior.cs
 * Created by: Neo, Ambrosia
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: pikmin ai
 */

using UnityEngine;

public class PikminBehavior : MonoBehaviour, IPooledObject
{
    // Enumerations
    public enum States { Idle, Formation, Attacking }
    public enum Colour { Red, Blue, Yellow }

    //[Header("Components")]
    CharacterController _Controller;
    Player _Player;

    [Header("Settings")]
    [SerializeField] float _MovementSpeed = 5f;
    [SerializeField] float _Gravity = 15;

    [HideInInspector] public States _State;
    Vector3 _BaseHeight;

    void IPooledObject.OnObjectSpawn()
    {
        // Grab components
        _Controller = GetComponent<CharacterController>();
        _Player = Player.player;

        // Set required variables
        _State = States.Idle;
        _BaseHeight = Vector3.up * (_Controller.height / 2);
    }

    void Update()
    {
        // Handle slope physics and gravity
        if (!IsGrounded())
            _Controller.Move(Vector3.down * _Gravity * Time.deltaTime);

        switch (_State)
        {
            case States.Idle:
                // Would play idle animation,
                // check surroundings for things to do
                break;
            case States.Formation:
                MoveTowards(_Player.transform.position);
                break;
            case States.Attacking:

                break;
        }
    }

    void MoveTowards(Vector3 position)
    {
        // Get the target direction we want to move in, set the Y velocity to 0
        // so we don't glide into the air, and move the Pikmin
        Vector3 direction = position - transform.position;
        direction.y = 0;
        _Controller.Move(direction.normalized * _MovementSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        if (_Controller.isGrounded)
            return true;

        // Calculate the bottom position of the character controller
        // then check if there is any collider beneath us
        if (Physics.Raycast(transform.position - _BaseHeight, Vector3.down, out RaycastHit hit, 1f))
        {
            // Check if the raycast hit a floor,
            // and then check the distance between the floor and the player
            if (hit.normal == Vector3.up)
                return hit.distance <= 0.2;

            // Move down but only the distance away, this cancels out the bouncing
            // effect that you can achieve by removing this function
            _Controller.Move(Vector3.down * hit.distance);
            return true;
        }

        // We couldn't ground ourselves whilst travelling down a slope and 
        // the controller says we're not grounded so return false
        return false;
    }
}
