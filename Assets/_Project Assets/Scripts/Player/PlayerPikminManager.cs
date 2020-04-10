/*
 * PlayerPikminManager.cs
 * Created by: Ambrosia
 * Created on: 12/2/2020 (dd/mm/yy)
 * Created for: managing the Players Pikmin and the associated data
 */

using UnityEngine;
using System.Collections.Generic;

public class PlayerPikminManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform _FormationCenter;

    [Header("Throwing")]
    [SerializeField] float _PikminGrabRadius = 5;
    [SerializeField] float _VerticalMaxGrabRadius = 1.5f;
    [SerializeField] float _ThrowingGravity = Physics.gravity.y;
    [SerializeField] float _LaunchAngle = 50.0f;
    [SerializeField] Transform _WhistleTransform = null;

    [Header("Formation")]
    [SerializeField] float _StartingOffset;
    [SerializeField] float _DistancePerPikmin; // How much is added to the offset for each pikmin

    List<GameObject> _PikminOnField = new List<GameObject>();   // How many Pikmin there are currently alive
    List<GameObject> _Squad = new List<GameObject>();           // How many Pikmin there are currently in the Player's squad

    GameObject _PikminInHand;

    void Update()
    {
        HandleThrowing();
        HandleFormation();
    }

    void OnDrawGizmosSelected()
    {
        // Draw the formation center position
        Gizmos.DrawWireSphere(_FormationCenter.position, 1);
    }

    /// <summary>
    /// Handles throwing the Pikmin including arc calculation
    /// </summary>
    void HandleThrowing()
    {
        // Check if we've got more than 0 Pikmin in
        // our squad and we press the Throw key (currently Space)

        if (Input.GetButtonDown("ThrowPikmin") && GetPikminOnFieldCount() > 0)
        {
            GameObject closestPikmin = GetClosestPikmin();
            // Check if we've even gotten a Pikmin
            if (closestPikmin != null)
            {
                _PikminInHand = closestPikmin;
                _PikminInHand.GetComponent<PikminBehavior>().ThrowHoldStart();
            }
        }

        // The rest of the throw depends if we even got a Pikmin
        if (_PikminInHand != null)
        {
            if (Input.GetButton("ThrowPikmin"))
            {
                // Move the Pikmin's model to in front of the player
                _PikminInHand.transform.position = transform.position + (transform.forward / 2);
            }
            if (Input.GetButtonUp("ThrowPikmin"))
            {
                /*
                 * TODO: convert to Quadratic Bezier curve
                 */
                _PikminInHand.GetComponent<PikminBehavior>().ThrowHoldEnd();

                // Cache the Rigidbody component
                var rigidbody = _PikminInHand.GetComponent<Rigidbody>();

                // Use X and Z coordinates to calculate distance between Pikmin and whistle                
                Vector3 whistlePos = new Vector3(_WhistleTransform.position.x, 0, _WhistleTransform.position.z);
                Vector3 pikiPos = new Vector3(_PikminInHand.transform.position.x, 0, _PikminInHand.transform.position.z);

                // Calculate vertical and horizontal distance between Pikmin and whistle
                float vd = _WhistleTransform.position.y - _PikminInHand.transform.position.y;
                float d = Vector3.Distance(pikiPos, whistlePos);

                // Plug the variables into the equation...
                float g = _ThrowingGravity;
                float angle = Mathf.Deg2Rad * _LaunchAngle;

                // Calculate horizontal and vertical velocity
                float velX = Mathf.Sqrt(g * d * d / (2.0f * (vd - (d * Mathf.Tan(angle)))));
                float velY = velX * Mathf.Tan(angle);

                // Face whistle and convert local velocity to global, and apply it
                transform.LookAt(new Vector3(whistlePos.x, transform.position.y, whistlePos.z));
                _PikminInHand.transform.LookAt(new Vector3(whistlePos.x, _PikminInHand.transform.position.y, whistlePos.z));
                rigidbody.velocity = _PikminInHand.transform.TransformDirection(new Vector3(0.0f, velY, velX));

                // TODO: Adjust targeting to be more accurate to whistle position/avoid having Pikmin
                // be thrown directly in front of Olimar rather than onto the whistle.


                // As the Pikmin has been thrown, remove it from the hand variable
                _PikminInHand = null;
            }
        }

        // (test) Killing the Pikmin
        if (Input.GetKeyDown(KeyCode.B) && GetPikminOnFieldCount() > 0)
        {
            GameObject closestPikmin = GetClosestPikmin();
            if (closestPikmin != null)
            {
                var pikminComponent = closestPikmin.GetComponent<PikminBehavior>();
                pikminComponent.ChangeState(PikminBehavior.States.Dead);
            }
        }
    }

    /// <summary>
    /// Prevents the Pikmin formation center from moving every frame
    /// by clamping it to a set distance away from the player
    /// </summary>
    void HandleFormation()
    {
        Vector3 targetPosition = (_FormationCenter.position - transform.position).normalized;
        _FormationCenter.position = transform.position + Vector3.ClampMagnitude(targetPosition, _StartingOffset + _DistancePerPikmin * _Squad.Count);
    }


    /// <summary>
    /// Searches for the closest Pikmin in proximity to the Player and returns it
    /// </summary>
    /// <returns>The closest Pikmin gameobject in the Player's squad</returns>
    GameObject GetClosestPikmin()
    {
        GameObject closestPikmin = null;
        float closestPikminDistance = _PikminGrabRadius;

        // Grab all colliders within a given radius from our current position
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _PikminGrabRadius);
        foreach (var collider in hitColliders)
        {
            // Check if the collider is actually a pikmin
            var pikminComponent = collider.GetComponent<PikminBehavior>();
            if (pikminComponent != null)
            {
                // Check if they're in the squad
                if (pikminComponent.GetState() != PikminBehavior.States.Formation)
                    continue;

                // Vertical check, make sure Pikmin don't get thrown if too far up
                // or downwards from the position of the Player
                float verticalDistance = Mathf.Abs(transform.position.y - collider.transform.position.y);
                if (verticalDistance > _VerticalMaxGrabRadius)
                    continue;

                // Assign it on our first run
                if (closestPikmin == null)
                {
                    closestPikmin = collider.gameObject;
                    continue;
                }

                // Checks the distance between the previously checked Pikmin
                // and the Pikmin we're doing now
                float distanceToPlayer = Vector3.Distance(collider.transform.position, transform.position);
                if (distanceToPlayer < closestPikminDistance)
                {
                    closestPikmin = collider.gameObject;
                    closestPikminDistance = distanceToPlayer;
                }
            }
        }

        return closestPikmin;
    }

    #region Global Setters
    public void AddPikminOnField(GameObject toAdd) => _PikminOnField.Add(toAdd);
    public void AddToSquad(GameObject toAdd) => _Squad.Add(toAdd);
    public void RemovePikminOnField(GameObject toRem) => _PikminOnField.Remove(toRem);
    public void RemoveFromSquad(GameObject toRem) => _Squad.Remove(toRem);
    #endregion

    #region Global Getters
    public List<GameObject> GetPikminOnField() => _PikminOnField;
    public int GetPikminOnFieldCount() => _PikminOnField.Count;
    public int GetSquadCount() => _Squad.Count;
    public Transform GetFormationCenter() => _FormationCenter;
    #endregion
}
