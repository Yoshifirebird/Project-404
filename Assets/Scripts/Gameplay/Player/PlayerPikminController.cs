/*
 * PlayerPikminController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerPikminController : MonoBehaviour {

  void Update () {
    if (Input.GetButtonDown ("X Button")) {
      PikminStatsManager.ClearSquad ();
    }
  }

  void OnGUI () {
    if (GameManager._DebugGui) {
      int yOffset = 200;
      GUI.Label (new Rect (10, yOffset + 10, 300, 500), PikminStatsManager._RedStats.ToString ());
      GUI.Label (new Rect (10, yOffset + 80, 300, 500), PikminStatsManager._YellowStats.ToString ());
      GUI.Label (new Rect (10, yOffset + 150, 300, 500), PikminStatsManager._BlueStats.ToString ());
    }
  }
}
