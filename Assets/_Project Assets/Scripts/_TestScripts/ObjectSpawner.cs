/*
 * PLACEHOLDER_PikminBehavior.cs
 * Created by: Neo
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Testing the pooling system by spawning objects from a pool at a set location.
 */

using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] string _PoolTag;
    [SerializeField] Vector3 _SpawnLocation;
    [SerializeField] int _Amount;

    ObjectPooler _ObjectPooler;

    void Start()
    {
        // Grab the ObjectPooler instance
        _ObjectPooler = ObjectPooler.Instance;

        for (int i = 0; i < _Amount; i++)
            // Call the SpawnFromPool function and spawn objects in the tagged pool at the spawn location
            _ObjectPooler.SpawnFromPool(_PoolTag, _SpawnLocation + (Vector3.back / 2) * i, Quaternion.identity);
    }
}
