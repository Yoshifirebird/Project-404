/*
 * PlayerPikminController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerPikminController : MonoBehaviour
{

  [Header("Throwing")]
  [SerializeField] float _PikminGrabRadius = 5;
  [SerializeField] float _MaxGrabHeight = 1;

  [Header("Debug")]
  [SerializeField] int _GrabFXQuality = 50;
  [SerializeField] GameObject _HoldingPikmin = null;

  void Update()
  {
    if (Input.GetButtonDown("X Button"))
    {
      PikminStatsManager.ClearSquad();
    }

    bool attemptedGrab = false;

    if (Input.GetButtonDown("A Button"))
    {
      attemptedGrab = true;
      if (PikminStatsManager.GetTotalInSquad() > 0)
      {
        _HoldingPikmin = GetClosestPikmin();
      }
    }

    if (_HoldingPikmin != null)
    {
      // TODO: Add holding and throwing
    }
    else if (attemptedGrab)
    {
      // We couldn't grab a Pikmin, we can punch now

      print("Starting punch");
    }
  }

  void OnGUI()
  {
    if (GameManager._DebugGui)
    {
      int yOffset = 210;
      GUI.Label(new Rect(10, yOffset, 300, 500), PikminStatsManager._RedStats.ToString());
      GUI.Label(new Rect(10, yOffset + 70, 300, 500), PikminStatsManager._YellowStats.ToString());
      GUI.Label(new Rect(10, yOffset + 140, 300, 500), PikminStatsManager._BlueStats.ToString());
    }
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.green;
    for (int i = 0; i < _GrabFXQuality; i++)
    {
      Vector3 endPosition = transform.position + MathUtil.XZToXYZ(_PikminGrabRadius * MathUtil.PositionInUnit(_GrabFXQuality, i));
      
      // Visualise the upper bounds of the grab height limitation
      Gizmos.DrawLine(endPosition, endPosition + (Vector3.up * _MaxGrabHeight));
      Gizmos.DrawLine(endPosition + (Vector3.up * _MaxGrabHeight), transform.position);

      // Visualise the lower bounds of the grab height limitation
      Gizmos.DrawLine(endPosition, endPosition + (Vector3.down * _MaxGrabHeight));
      Gizmos.DrawLine(endPosition + (Vector3.down * _MaxGrabHeight), transform.position);
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
      if (yDist >= _MaxGrabHeight || yDist <= -_MaxGrabHeight)
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
