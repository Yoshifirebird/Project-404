/*
 * FadeManager.cs
 * Created by: Ambrosia
 * Created on: 26/4/2020 (dd/mm/yy)
 * Created for: Needing an object that can smoothly transition between events with a coloured backdrop overlaying the screen
 */

using UnityEngine;

public class FadeManager : MonoBehaviour {
    //[Header("Components")]

    //[Header("Settings")]

    private void Awake () {
        DontDestroyOnLoad (this);
    }

    private void Update () {

    }
}