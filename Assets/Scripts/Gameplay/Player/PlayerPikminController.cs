/*
 * PlayerPikminController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerPikminController : MonoBehaviour {

  void Update () {
    if (Input.GetButtonDown ("X Button")) {
      while (PikminStatsManager._InSquad.Count > 0) {
        var pikminInSquad = PikminStatsManager._InSquad[0];
        pikminInSquad.GetComponent<PikminAI> ().RemoveFromSquad ();
      }
    }
  }

  void OnGUI () {
    if (GameManager._DebugGui) {
      GUI.Label (new Rect (10, 10, 300, 500), PikminStatsManager._RedStats.ToString ());
      GUI.Label (new Rect (10, 80, 300, 500), PikminStatsManager._YellowStats.ToString ());
      GUI.Label (new Rect (10, 150, 300, 500), PikminStatsManager._BlueStats.ToString ());
    }
  }
}
