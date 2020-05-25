/*
 * PlayerUIController.cs
 * Created by: Ambrosia
 * Created on: 25/5/2020 (dd/mm/yy)
 */

using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {
  [Header ("Components")]
  [SerializeField] Text _TextDay = null;
  [SerializeField] Text _HealthText = null;
  [SerializeField] Image _HealthBar = null;
  Player _Player = null;

  [Header ("Settings")]
  [SerializeField] float _HealthBarTransitionRate = 5;

  void Start () {
    _TextDay.text = DayManager._CurrentDay.ToString ();
    _Player = GameManager._Player;
  }

  void Update () {
    _HealthBar.fillAmount = Mathf.Lerp (_HealthBar.fillAmount, _Player.GetCurrentHealth () / _Player.GetMaxHealth (), _HealthBarTransitionRate * Time.deltaTime);
    _HealthText.text = ((int) _Player.GetCurrentHealth ()).ToString ();
  }
}
