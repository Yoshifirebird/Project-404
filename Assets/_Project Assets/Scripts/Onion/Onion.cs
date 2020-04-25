/*
 * Onion.cs
 * Created by: Ambrosia
 * Created on: 12/4/2020 (dd/mm/yy)
 * Created for: Needing an object to store our Pikmin in
 */

using UnityEngine;

public class Onion : MonoBehaviour {
    public enum OnionType {
        Classic, // When first finding an onion, it will be this
        Master // Main onion that has the combination of other onions
    }

    //[Header("Components")]

    [Header ("Settings")]
    [SerializeField] OnionType _Type = OnionType.Classic;
    [SerializeField] Colour _Colour = Colour.Red;

    bool _CanUse = false;
    bool _InMenu = false;

    void Update () {
        if (_CanUse && Input.GetButtonDown ("A Button")) {
            // Set the menu to the opposite of what it just was (true -> false || false -> true)
            // TODO: Pause player input and the world
            _InMenu = !_InMenu;

            if (_InMenu == false) {
                print ("Closing Onion menu");
                // TODO: Play animation where the UI goes away
                return;
            }

            print ("Opening Onion menu");
        }

        // Handle in-menu input processing
        if (_InMenu) {
            // TODO: UI Animations for changing the text value
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("Player")) {
            _CanUse = true;
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.CompareTag ("Player")) {
            _CanUse = false;
        }
    }
}