/*
 * PlayerPikminController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerPikminController : MonoBehaviour
{

  void Update()
  {
    if (Input.GetButtonDown("X Button"))
    {
      while (PikminStatsManager._InSquad.Count > 0)
      {
        var pikminInSquad = PikminStatsManager._InSquad[0];
        pikminInSquad.GetComponent<PikminAI>().RemoveFromSquad();
      }
    }
  }

  void OnGUI()
  {
    Rect currentPosition = new Rect(50, 50, 50, 50);
    GUI.Label(currentPosition, "hello");
  }
}
