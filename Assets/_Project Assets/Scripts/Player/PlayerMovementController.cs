/*
 * PlayerMovementController.cs
 * Created by: Ambrosia
 * Created on: 7/2/2020 (dd/mm/yy)
 * Created for: controlling the movement of the Player
 */

using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    //[Header("Components")]
    CharacterController _Controller;
    Camera _MainCamera;

    [Header("Settings")]
    [SerializeField] float _MovementSpeed = 3;
    [SerializeField] Vector2 _MovementDeadzone;
    [SerializeField] float _RotationSpeed = 3;
    [SerializeField] float _Gravity = -Physics.gravity.y;

    private void Awake()
    {
        _Controller = GetComponent<CharacterController>();
        _MainCamera = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // If we're not grounded and not on a slope
        if (!IsGrounded())
        {
            // Work out our gravitational Y velocity and apply it
            float yVelocity = _Gravity * Time.deltaTime;
            _Controller.SimpleMove(Vector3.down * yVelocity * Time.deltaTime);
        }

        // Get input from the 'Horizontal' and 'Vertical' axis, and normalize it
        // so as to not the player move quicker when going diagonally
        var mDirection = new Vector3(
                                        Input.GetAxis("Horizontal"),
                                        0,
                                        Input.GetAxis("Vertical")
                                        ).normalized;

        // If the player has even touched the H and V axis
        if (Mathf.Abs(mDirection.x) <= _MovementDeadzone.x && mDirection.z <= Mathf.Abs(_MovementDeadzone.y))
            return;

        // Make the movement vector relative to the camera's position/rotation
        // and remove any Y momentum gained from doing the TransformDirection
        mDirection = _MainCamera.transform.TransformDirection(mDirection);
        mDirection.y = 0;

        // Rotate and move the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(mDirection), _RotationSpeed * Time.deltaTime);
        _Controller.Move(mDirection.normalized * _MovementSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        if (_Controller.isGrounded)
            return true;

        // Calculate the bottom position of the character controller
        // then check if there is any collider beneath us
        Vector3 bottom = transform.position - Vector3.up * (_Controller.height / 2);
        if (Physics.Raycast(bottom, Vector3.down, out RaycastHit hit, 0.5f))
        {
            // If there is, move down but only the distance away, this creates a slope-like effect
            // cancelling out the bouncing found if you remove this function
            _Controller.Move(Vector3.down * hit.distance);
            return true;
        }

        // We couldn't ground ourselves whilst travelling down a slope and 
        // the controller says we're not grounded so return false
        return false;
    }
}
