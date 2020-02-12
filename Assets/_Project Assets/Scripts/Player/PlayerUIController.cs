/*
 * PlayerUIController.cs
 * Created by: Ambrosia
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: needing a controller for the UI to show appropriate variables
 * Notes: Dependant on the 'Player' component
 */

using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Image _HealthCircle;

	[SerializeField] Text _TotalPikminText;
	[SerializeField] Text _InSquadText;
	[SerializeField] Text _OnFieldText;
	[SerializeField] Text _DayText;

	Player _Player;
	PlayerPikminManager _PikminManager;
	int _MaxHealth;

	//[Header("Settings")]


	void Awake()
	{
		_Player = GetComponent<Player>();
		_PikminManager = GetComponent<PlayerPikminManager>();
		_MaxHealth = _Player.GetMaxHealth();
	}

	void Update()
	{
		SetHealthCircle();
		SetPikminStats();
	}

	void SetHealthCircle()
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			_Player.TakeHealth(10);
		}

		float step = (float)_Player.GetHealth() / (float)_MaxHealth;
		// Sets the fill amount to be a fraction (0 - dead, _MaxHealth - alive)
		_HealthCircle.fillAmount = step;
		_HealthCircle.color = Color.Lerp(Color.red, Color.green, step);
	}

	void SetPikminStats()
	{
		_TotalPikminText.text = PlayerStats._TotalPikmin.ToString();
		_DayText.text = PlayerStats._Day.ToString();
		_InSquadText.text = _PikminManager.GetSquadCount().ToString();
		_OnFieldText.text = _PikminManager.GetPikminOnField().ToString();
	}
}
