/*
 * Water.cs
 * Created by: Ambrosia
 * Created on: 14/3/2020 (dd/mm/yy)
 * Created for: needing Pikmin to start drowning when entering Water
 */

using UnityEngine;

public class Water : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Handle (TODO) Water splash / audio for object entering water
        var rbComponent = other.GetComponent<Rigidbody>();
        if (rbComponent != null)
        {
            //Debug.Log("Object came into water at velocity of " + rbComponent.velocity.ToString());
        }

        // Handle Pikmin entering the water
        var pikminComponent = other.GetComponent<PikminBehavior>();
        if (pikminComponent != null)
        {
            pikminComponent.EnterWater();
            return;
        }

        // Handle Player entering the water
        var pComponent = other.GetComponent<PlayerMovementController>();
        if (pComponent == null)
            return;
        //print(pComponent._gVelocity);
    }
}
