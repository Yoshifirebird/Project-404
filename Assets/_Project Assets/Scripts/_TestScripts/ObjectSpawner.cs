/*
 * PLACEHOLDER_PikminBehavior.cs
 * Created by: Neo
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Testing the pooling system by spawning objects from a pool at a set location.
 */

using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] string _PoolTag;

    [Header("Spawn Settings")]
    [SerializeField] bool _UseCurrentPosition = false;
    [SerializeField] Vector3 _SpawnLocation;
    [SerializeField] int _Amount;
    [SerializeField] float _Stagger;

    ObjectPooler _ObjectPooler;

    void Start()
    {
        // Grab the ObjectPooler instance
        _ObjectPooler = ObjectPooler.Instance;

        Vector3 position = _UseCurrentPosition ? transform.position : _SpawnLocation;
        float stagger = _Stagger;
        for (int i = 0; i < _Amount; i++)
        {
            // Call the SpawnFromPool function and spawn objects in the tagged pool at the spawn location
            _ObjectPooler.SpawnFromPool(_PoolTag, position + (Vector3.back * stagger), Quaternion.identity);
            // Add more space between the initial object and the new object
            stagger += _Stagger;
        }
    }
}
