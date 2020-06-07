using UnityEngine;
using UnityEngine.UI;

public class UIPikminCounter : MonoBehaviour {
  [SerializeField] Text _OnFieldText;
  [SerializeField] Animation _OnFieldChangeAnimation;
  [SerializeField] Text _InSquadText;
  [SerializeField] Animation _InSquadChangeAnimation;
  [SerializeField] ColorLerper _Flasher;

  void PlayChangeAnimation(bool onField)
  {
    if (onField)
    {
      if (_OnFieldChangeAnimation.isPlaying)
      {
        _OnFieldChangeAnimation.Stop();
      }
      _OnFieldChangeAnimation.Play();
    }
    else
    {
      if (_InSquadChangeAnimation.isPlaying)
      {
        _InSquadChangeAnimation.Stop();
      }
      _InSquadChangeAnimation.Play();
    }
  }

  void Update () {
    int areaValue = PikminStatsManager.GetTotalOnField();
    int squadValue = PikminStatsManager.GetTotalInSquad();

    if (_OnFieldText.text != areaValue.ToString ()) {
      PlayChangeAnimation(true);
      _OnFieldText.text = areaValue.ToString ();
    }

    if (_InSquadText.text != squadValue.ToString ()) {
      PlayChangeAnimation(false);
      _InSquadText.text = squadValue.ToString ();
    }

    _Flasher.index = areaValue >= 100 ? 1 : 0;
  }
}
