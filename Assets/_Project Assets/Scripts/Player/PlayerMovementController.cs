/*
 * PlayerMovementController.cs
 * Created by: Ambrosia
 * Created on: 7/2/2020 (dd/mm/yy)
 * Created for: controlling the movement of the Player
 */

using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform _WhistleTransform;
    CharacterController _Controller;
    Camera _MainCamera;

    [Header("Settings")]
    [SerializeField] float _MovementSpeed = 3;
    [SerializeField] Vector2 _MovementDeadzone;
    [SerializeField] float _RotationSpeed = 3;
    [SerializeField] float _Gravity = -Physics.gravity.y;

    [SerializeField] float _LookAtWhistleTime = 2.5f;

    [HideInInspector] public Quaternion _RotationBeforeIdle;
    float _IdleTimer = 0;

    Vector3 _BaseHeight;

    private void Awake()
    {
        _Controller = GetComponent<CharacterController>();
        _BaseHeight = Vector3.up * (_Controller.height / 2);
        _MainCamera = Camera.main;
    }

    private void Update()
    {
        // If we're not grounded and not on a slope
        if (!IsGrounded())
            // Apply gravity
            _Controller.Move(Vector3.down * _Gravity * Time.deltaTime);

        // Add time to the idle timer
        _IdleTimer += Time.deltaTime;

        // Check if we've been idle long enough to look at the whistle
        if (_IdleTimer > _LookAtWhistleTime)
        {
            Vector3 finalLookPosition = _WhistleTransform.position - transform.position;
            finalLookPosition.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(finalLookPosition), _RotationSpeed * Time.deltaTime);
        }
        else
        {
            _RotationBeforeIdle = transform.rotation;
        }

        // Get input from the 'Horizontal' and 'Vertical' axis, and normalize it
        // so as to not the player move quicker when going diagonally
        var mDirection = new Vector3(Input.GetAxis("Horizontal"),
                                     0,
                                     Input.GetAxis("Vertical")).normalized;

        // If the player has even touched the H and V axis
        if (Mathf.Abs(mDirection.x) <= _MovementDeadzone.x && Mathf.Abs(mDirection.z) <= _MovementDeadzone.y)
            return;

        // We've moved, so the idle timer gets reset
        _IdleTimer = 0;

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
