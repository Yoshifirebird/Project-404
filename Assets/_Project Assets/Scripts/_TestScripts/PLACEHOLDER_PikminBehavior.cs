/*
 * PLACEHOLDER_PikminBehavior.cs
 * Created by: Neo
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Testing the pooling system using a placeholder object.
 * Notes: Please delete after we implement PROPER Pikmin Behavior.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLACEHOLDER_PikminBehavior : MonoBehaviour, IPooledObject
{
    void IPooledObject.OnObjectSpawn()
    {
        // Do whatever.
        Debug.Log("I spawned!");
        transform.rotation = Quaternion.Euler(Mathf.Round(Random.Range(0, 80)), 0, Mathf.Round(Random.Range(0, 80)));
    }
}
