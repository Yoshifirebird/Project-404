/*
 * PlayerPikminController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerPikminController : MonoBehaviour {

  [Header("Throwing")]
  [SerializeField] float _PikminGrabRadius = 5;
  [SerializeField] float _MaxGrabHeight = 1;

  void Update () {
    if (Input.GetButtonDown ("X Button")) {
      PikminStatsManager.ClearSquad ();
    }
    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      print(GetClosestPikmin());
    }
  }

  void OnGUI () {
    if (GameManager._DebugGui) {
      int yOffset = 210;
      GUI.Label (new Rect (10, yOffset, 300, 500), PikminStatsManager._RedStats.ToString ());
      GUI.Label (new Rect (10, yOffset + 70, 300, 500), PikminStatsManager._YellowStats.ToString ());
      GUI.Label (new Rect (10, yOffset + 140, 300, 500), PikminStatsManager._BlueStats.ToString ());
    }
  }

  GameObject GetClosestPikmin()
  {
    // C = closest, Pik = Pikmin, Dist = Distance
    GameObject cPik = null;
    float cPikDist = _PikminGrabRadius;

    foreach (var pikmin in PikminStatsManager._InSquad)
    {
      // Check the distance between the Pikmin and the Player
      float pikDist = MathUtil.DistanceTo(transform.position, pikmin.transform.position, false);
      if (pikDist >= _PikminGrabRadius * _PikminGrabRadius)
      {
        continue;
      }

      float yDist = Mathf.Abs(transform.position.y - pikmin.transform.position.y);
      if (yDist < _MaxGrabHeight)
      {
        continue;
      }

      if (pikDist < cPikDist)
      {
        cPikDist = pikDist;
        cPik = pikmin;
      }
    }

    return cPik;
  }
}
