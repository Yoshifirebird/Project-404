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

    private void HandleMovement()
    {
        if (!IsGrounded())
        {
            // Get the Y velocity we're about to apply
            float yVelocity = _Gravity * Time.deltaTime;
            // Apply a downwards movement using the Y velocity
            _Controller.SimpleMove(Vector3.down * yVelocity * Time.deltaTime);
        }

        // Get input from the 'Horizontal' and 'Vertical' axis, and normalize it to not let
        // the player move quicker when going diagonally
        var mDirection = new Vector3(
                                        Input.GetAxis("Horizontal"),
                                        0,
                                        Input.GetAxis("Vertical")
                                        ).normalized;

        // If the player has even touched the H and V axis
        if (mDirection.x == 0 && mDirection.z == 0)
            return;

        // Make the movement vector relative to the camera's position/rotation
        mDirection = _MainCamera.transform.TransformDirection(mDirection);
        // Remove any Y momentum gained when doing the TransformDirection
        mDirection.y = 0;

        // Apply the movement using '_MovementSpeed' as a multiplier
        _Controller.Move(mDirection * _MovementSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        // If the character controller says we're grounded
        if (_Controller.isGrounded)
            // We're grounded
            return true;

        // Calculate the bottom position of the character controller
        Vector3 bottom = transform.position - Vector3.up * (_Controller.height / 2);
        // Check if there is anything beneath us, with a max distance of 0.3 units
        if (Physics.Raycast(bottom, Vector3.down, out RaycastHit hit, 0.3f))
        {
            // If there is, move down but only the distance away, this creates a slope-like effect
            // cancelling out the bouncing found if you remove this function
            _Controller.Move(Vector3.down * hit.distance);
            // We're now grounded
            return true;
        }

        // We couldn't ground ourselves whilst travelling down a slope
        // and the controller says we're not, so I guess we aren't grounded
        return false;
    }
}
