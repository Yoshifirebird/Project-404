/*
 * PlayerUIController.cs
 * Created by: Ambrosia & Newgame+ LD
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: needing a controller for the UI to show appropriate variables
 * Notes: Dependant on the 'Player' component
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {
    [Header ("Components")]
    [SerializeField] Image _HealthCircle;
    [SerializeField] Gradient _HealthColor;
    float _ReferenceHealth; //Newgame+ LD: This is to track if the player has lost or gained HP
    float _MaxHealth;

    [SerializeField] TextMeshProUGUI _TotalPikminText;
    [SerializeField] TextMeshProUGUI _InSquadText;
    [SerializeField] TextMeshProUGUI _OnFieldText;
    [SerializeField] TextMeshProUGUI _DayText;

    Player _Player;
    PlayerPikminManager _PikminManager;

    void Start () {
        _Player = Globals._Player;
        _PikminManager = _Player.GetPikminManager ();
        _MaxHealth = _Player.GetMaxHealth ();
        _ReferenceHealth = _Player.GetHealth ();
    }

    void Update () {
        SetHealthCircle ();
        SetPikminStats ();
    }

    void SetHealthCircle () {
        if (_Player.GetHealth () < _ReferenceHealth) {
            print ("Ouch! I lost some HP!");
        }
        // Update health as it may have been decreased since the last frame
        _ReferenceHealth = _Player.GetHealth ();

        float step = (float) _ReferenceHealth / (float) _MaxHealth;
        _HealthCircle.fillAmount = Mathf.MoveTowards (_HealthCircle.fillAmount, step, Time.deltaTime);
        _HealthCircle.color = _HealthColor.Evaluate (step);
    }

    void SetPikminStats () {
        _TotalPikminText.text = PlayerStats._TotalPikmin.ToString ();
        _DayText.text = PlayerStats._Day.ToString ();
        _InSquadText.text = PlayerStats._InSquad.Count.ToString ();
        _OnFieldText.text = PlayerStats._OnField.Count.ToString ();
    }
}