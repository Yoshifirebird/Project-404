/*
 * Test_PikminStats.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public class Test_PikminStats : MonoBehaviour {
  [Header ("Settings")]
  [SerializeField] KeyCode _KeyToApply = KeyCode.Alpha1;
  [SerializeField] KeyCode _KeyToPrint = KeyCode.Alpha0;
  [SerializeField] GameObject _Pikmin;

  void Update () {
    if (Input.GetKeyDown (_KeyToApply)) {
      Instantiate (_Pikmin, transform.position, Quaternion.identity);
    }

    if (Input.GetKeyDown (_KeyToPrint)) {
      PikminStatsManager.Print ();
    }
  }
}
