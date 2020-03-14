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
        // Handle Pikmin entering the water
        var pikminComponent = other.GetComponent<PikminBehavior>();
        if (pikminComponent != null)
        {
            pikminComponent.EnterWater();
        }
    }
}
