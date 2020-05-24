using UnityEngine;
using UnityEngine.UI;

public class UIPikminCounter : MonoBehaviour {
  [SerializeField] Text _OnFieldText;
  [SerializeField] Text _InSquadText;

  void Update () {
    _OnFieldText.text = (PikminStatsManager.GetOnField (PikminColour.Red) +
      PikminStatsManager.GetOnField (PikminColour.Yellow) +
      PikminStatsManager.GetOnField (PikminColour.Blue)).ToString ();
    _InSquadText.text = (PikminStatsManager.GetInSquad (PikminColour.Red) +
      PikminStatsManager.GetInSquad (PikminColour.Yellow) +
      PikminStatsManager.GetInSquad (PikminColour.Blue)).ToString ();
  }
}
