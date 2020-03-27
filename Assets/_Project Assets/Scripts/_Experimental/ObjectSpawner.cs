/*
 * ObjectSpawner.cs
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
    [SerializeField] Vector3 _SpawnLocation = Vector3.one;
    [SerializeField] int _Amount = 1;
    [SerializeField] float _Stagger = 1;

    ObjectPooler _ObjectPooler;

    void Start()
    {
        // Grab the ObjectPooler instance
        _ObjectPooler = ObjectPooler.Instance;

        float stagger = _Stagger;
        for (int i = 0; i < _Amount; i++)
        {
            // Spawn objects in the tagged pool at the spawn location
            _ObjectPooler.SpawnFromPool(_PoolTag, _SpawnLocation + (-transform.forward * stagger), Quaternion.identity);
            // Add more space between the initial object and the new object
            stagger += _Stagger;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Change the SpawnLocation in the Inspector
        if (_UseCurrentPosition)
            _SpawnLocation = transform.position;
    }
}
