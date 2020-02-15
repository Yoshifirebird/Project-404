using System;
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
    [SerializeField] float _Angle = 50.0f;
    [SerializeField] Transform _WhistleTransform;

    List<GameObject> _PikminOnField = new List<GameObject>(); // // How many Pikmin there are currently alive
    List<GameObject> _Squad = new List<GameObject>();        // How many Pikmin there are currently in the Player's squad

    GameObject _PikminInHand;

    private void Update()
    {
        HandleThrowing();
    }

    void HandleThrowing()
    {
        // Check if we've got more than 0 Pikmin in
        // our squad and we press the Throw key (currently Space)
        if (Input.GetKeyDown(KeyCode.Space) && GetPikminOnFieldCount() > 0)
        {
            GameObject closestPikmin = null;
            float closestPikminDistance = _PikminGrabRadius;

            // Grab the colliders using our position and a given radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _PikminGrabRadius);
            foreach (var collider in hitColliders)
            {
                // Check if the collider's gameobject is a pikmin
                var pikminComponent = collider.GetComponent<PikminBehavior>();
                if (pikminComponent != null)
                {
                    // Check if they're even in the squad
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

                    // Grab the distance to the player, compare it against the current max and
                    // then  assign it based on the result of the if statement
                    float distanceToPlayer = Vector3.Distance(collider.transform.position, transform.position);
                    if (distanceToPlayer < closestPikminDistance)
                    {
                        closestPikmin = collider.gameObject;
                        closestPikminDistance = distanceToPlayer;
                    }
                }
            }

            // We've finally got the closest Pikmin
            if (closestPikmin != null)
            {
                _PikminInHand = closestPikmin;
            }
        }

        // The rest of the throwing code all depends if
        // the PikminInHand is null or not
        if (_PikminInHand != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _PikminInHand.transform.position = transform.position + transform.right;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Todo: do this part of the Pikmin throwing... 

                // As an example of what we can do, throw the Pikmin and 
                // change its state to thrown
                var pikminComponent = _PikminInHand.GetComponent<PikminBehavior>();
                pikminComponent.RemoveFromSquad();
                pikminComponent.ChangeState(PikminBehavior.States.Thrown);

                var rigidbody = _PikminInHand.GetComponent<Rigidbody>();

                //Use Vector3s with X and Z coordinates only to calculate distance between Pikmin and whistle                
                Vector3 whistlePos = new Vector3(_WhistleTransform.position.x, 0.0f, _WhistleTransform.position.z);
                Vector3 pikiPos = new Vector3(rigidbody.position.x, 0.0f, rigidbody.position.z);

                //Calculate vertical and horizontal distance between Pikmin and whistle
                float vd = _WhistleTransform.position.y - rigidbody.position.y;
                float d = Vector3.Distance(pikiPos, whistlePos);

                float g = Physics.gravity.y;                
                float angle = Mathf.Deg2Rad * _Angle;

                //Calculate horizontal and vertical velocity
                float velX = Mathf.Sqrt(g*d*d/(2.0f*(vd-d*Mathf.Tan(angle))));
                float velY = velX * Mathf.Tan(angle);
                
                //Face whistle and convert local velocity to global
                rigidbody.transform.LookAt(whistlePos);
                rigidbody.velocity = rigidbody.transform.TransformDirection(new Vector3(0.0f, velY, velX));  

                //TODO: Adjust targeting to be more accurate to whistle position/avoid having Pikmin
                //be thrown directly in front of Olimar rather than onto the whistle.
                
                _PikminInHand = null;
                
            }
        }
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
