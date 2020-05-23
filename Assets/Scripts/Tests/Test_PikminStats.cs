/*
 * NewBehaviourScript.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public class Test_PikminStats : MonoBehaviour {
  [Header ("Settings")]
  [SerializeField] KeyCode _KeyToApply = KeyCode.Alpha1;
  [SerializeField] KeyCode _KeyToPrint = KeyCode.Alpha0;

  [Header ("Modifiers")]
  [SerializeField] PikminMaturity _Maturity = default;
  [SerializeField] PikminColour _Colour = default;
  [SerializeField] PikminStatSpecifier _Specifier = default;
  [SerializeField] bool _Add = true;

  void Update () {
    if (Input.GetKeyDown (_KeyToApply)) {
      if (_Add) {
        PikminStatsManager.Add (_Colour, _Maturity, _Specifier);
      }
      else {
        PikminStatsManager.Remove (_Colour, _Maturity, _Specifier);
      }
    }
    
    if (Input.GetKeyDown(_KeyToPrint))
    {
      PikminStatsManager.Print ();
    }
  }
}
