using UnityEngine;
using UnityEngine.UI;

public class UIPikminCounter : MonoBehaviour {
  [SerializeField] Text _OnFieldText;
  [SerializeField] Text _InSquadText;
	[SerializeField] ColorLerper flasher;

  void Update () {

		int areaValue = (PikminStatsManager.GetOnField (PikminColour.Red) +
			PikminStatsManager.GetOnField (PikminColour.Yellow) +
			PikminStatsManager.GetOnField (PikminColour.Blue));
		int squadValue = (PikminStatsManager.GetInSquad (PikminColour.Red) +
			PikminStatsManager.GetInSquad (PikminColour.Yellow) +
			PikminStatsManager.GetInSquad (PikminColour.Blue));

			if(_OnFieldText.text != areaValue.ToString ())
			{
				_OnFieldText.gameObject.SetActive(false);	//Setting them inactive then active to make animation play
				_OnFieldText.text = areaValue.ToString ();

				_OnFieldText.gameObject.SetActive(true);
			}

			if(_InSquadText.text != squadValue.ToString ())
			{
				_InSquadText.gameObject.SetActive(false);
				_InSquadText.text = squadValue.ToString ();

				_InSquadText.gameObject.SetActive(true);
			}

		flasher.index = areaValue >= 100 ? 1 : 0;
  }
}
