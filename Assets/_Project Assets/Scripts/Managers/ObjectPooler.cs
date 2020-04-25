/*
 * ObjectPooler.cs
 * Created by: Neo
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Easy pooling of multiple, different objects for performance reasons
 */

using System.Collections;
using System.Collections.Generic; // Needed to be able to use Dictionaries
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    // This lets us have multiple types of pools and configure them accordingly in the inspector (Pikmin, generic objects, enemies, etc)
    [System.Serializable]
    public class Pool {
        public string _PoolTag; // Pool tag, or name
        public GameObject _PoolObjects; // Objects used in this pool
        public int _PoolSize; // The size of the pool
    }

    // Implements singleton design, since this is gonna be the only pooler we'll need
    #region Singleton

    public static ObjectPooler Instance;

    private void Awake () { Instance = this; }

    #endregion

    [SerializeField] List<Pool> _PoolList; // List of pools
    Dictionary<string, Queue<GameObject>> _PoolDictionary; // Make a dictionary of pools, kinda like an array but you can search using the string key

    void Start () {
        // Create a new dictionary
        _PoolDictionary = new Dictionary<string, Queue<GameObject>> ();

        // Add each pool to the Dictionary
        foreach (Pool pool in _PoolList) {
            // Create a new queue of GameObjects
            Queue<GameObject> objectPool = new Queue<GameObject> ();

            // Execute this until the pool is all filled up
            for (int i = 0; i < pool._PoolSize; i++) {
                // Create a new GameObject
                GameObject obj = Instantiate (pool._PoolObjects);
                // Set it inactive
                obj.SetActive (false);
                // Add it to the end of the queue
                objectPool.Enqueue (obj);
            }

            // Once all is said and done, add the newly created Pool to the Pool Dictionary
            _PoolDictionary.Add (pool._PoolTag, objectPool);
        }
    }

    // "Spawns" objects from the pool. Really it just moves them into position and activates them.
    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation) {
        // If the Pool Dictionary doesn't have a pool with the tag passed in
        if (!_PoolDictionary.ContainsKey (tag)) {
            // Log a warning, and return as null
            Debug.LogWarning ("Pool with tag \"" + tag + "\" doesn't exist!");
            return null;
        }

        // Find the first element in the queue, and set it as the object to spawn
        GameObject objectToSpawn = _PoolDictionary[tag].Dequeue ();
        objectToSpawn.SetActive (true); // Activate it
        objectToSpawn.transform.position = position; // Set it's position... 
        objectToSpawn.transform.rotation = rotation; // ...and it's rotation

        // Get the "IPooledObject" component from the object we're spawning
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject> ();
        // If the above doesn't return null
        if (pooledObj != null) {
            // Fire the "OnObjectSpawn" function
            pooledObj.OnObjectSpawn ();
        }

        // Enqueue the object to spawn once more, sending it to the back of the queue
        _PoolDictionary[tag].Enqueue (objectToSpawn);
        return objectToSpawn; // Return the object to spawn
    }

    // Stores objects back into the pool when we don't need them anymore instead of deleting them
    public void StoreInPool (string tag) {
        // Dequeues the object from the pool
        GameObject objectToStore = _PoolDictionary[tag].Dequeue ();
        objectToStore.transform.position = new Vector3 (0, 0, 0); // Sets the position to 0...
        objectToStore.transform.rotation = Quaternion.Euler (0, 0, 0); // ...and the rotation to 0 as well, as a way to reset the object
        objectToStore.SetActive (false); // Deactivates the object

        // Stores the object back into the Pool by sending it to the back of the queue
        _PoolDictionary[tag].Enqueue (objectToStore);
    }
}