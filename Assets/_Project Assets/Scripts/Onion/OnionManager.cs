/*
 * OnionManager.cs
 * Created by: Ambrosia
 * Created on: 12/4/2020 (dd/mm/yy)
 * Created for: needing a management system for the different onions
 */

using UnityEngine;

public class OnionManager : MonoBehaviour {
    //[Header("Components")]

    //[Header("Settings")]

    // This'll contain each onion type in an array the size of the Pikmin colours
    // 0 for locked, 1 for unlocked
    bool[] _UnlockedOnion;

    public static OnionManager _Manager;

    void Awake () {
        if (_Manager == null)
            _Manager = this;
        else
            Destroy (gameObject);

        _UnlockedOnion = new bool[(int) Colour.SIZE];
        for (int i = 0; i < (int) Colour.SIZE; i++) {
            // Sets all of the booleans to false if they aren't the first (red onion)
            _UnlockedOnion[i] = i == 0;
        }
    }

    public bool IsOnionUnlocked (Colour colour) {
        return _UnlockedOnion[(int) colour];
    }
}