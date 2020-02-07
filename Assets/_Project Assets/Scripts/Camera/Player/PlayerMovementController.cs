/*
 * PlayerMovementController.cs
 * Created by: Ambrosia
 * Created on: 7/2/2020 (dd/mm/yy)
 * Created for: controlling the movement of the Player
 */

using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // Uncomment these when there are variables in place
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
        // Get the Y velocity we're about to apply
        float yVelocity = _Gravity * Time.deltaTime;
        // Apply a downwards movement using the Y velocity
        _Controller.SimpleMove(Vector3.down * yVelocity * Time.deltaTime);

        // Get input from the 'Horizontal' and 'Vertical' axis, and normalize it to not let
        // the player move quicker when going diagonally
        var mDirection = new Vector3(Input.GetAxis("Horizontal"),
                             0,
                             Input.GetAxis("Vertical")).normalized;

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
}
