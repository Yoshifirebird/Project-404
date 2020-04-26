/*
 * OnionManager.cs
 * Created by: Ambrosia
 * Created on: 12/4/2020 (dd/mm/yy)
 * Created for: needing a management system for the different onions
 */

using UnityEngine;

public class OnionManager : MonoBehaviour {
    //[Header("Components")]

    [Header ("Settings")]
    // Goes Red, Yellow, Blue
    [SerializeField] bool[] _UnlockedOnion = new bool[(int) Colour.SIZE];
    // This'll contain each onion type in an array the size of the Pikmin colours
    // 0 for locked, 1 for unlocked ^^^ 

    void Awake () {
        if (Globals._OnionManager == null)
            Globals._OnionManager = this;
        else
            Destroy (gameObject);

        for (int i = 0; i < (int) Colour.SIZE; i++) {
            // Sets all of the booleans to false if they aren't the first (red onion)
            _UnlockedOnion[i] = i == (int) Colour.Red;
        }
    }

    public bool IsOnionUnlocked (Colour colour) {
        return _UnlockedOnion[(int) colour];
    }
}