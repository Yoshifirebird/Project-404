using UnityEngine;
using UnityEngine.UI;

public class UIPikminCounter : MonoBehaviour {
  [SerializeField] Text _OnFieldText;
  [SerializeField] Animation _OnFieldChangeAnimation;
  [SerializeField] Text _InSquadText;
  [SerializeField] Animation _InSquadChangeAnimation;
  [SerializeField] ColorLerper _Flasher;

  void Update () {

		int areaValue = (PikminStatsManager.GetOnField (PikminColour.Red) +
			PikminStatsManager.GetOnField (PikminColour.Yellow) +
			PikminStatsManager.GetOnField (PikminColour.Blue));
		int squadValue = (PikminStatsManager.GetInSquad (PikminColour.Red) +
			PikminStatsManager.GetInSquad (PikminColour.Yellow) +
			PikminStatsManager.GetInSquad (PikminColour.Blue));

		if(_OnFieldText.text != areaValue.ToString ())
		{
      _OnFieldChangeAnimation.Play();
			_OnFieldText.text = areaValue.ToString ();
		}

		if(_InSquadText.text != squadValue.ToString ())
		{
      _InSquadChangeAnimation.Play();
      _InSquadText.text = squadValue.ToString ();
		}

		_Flasher.index = areaValue >= 100 ? 1 : 0;
  }
}
