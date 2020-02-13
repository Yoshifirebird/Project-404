﻿/*
 * PlayerUIController.cs
 * Created by: Ambrosia & Newgame+ LD
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
	[SerializeField] Gradient _HealthColor;
	int _ReferenceHealth;	//Newgame+ LD: This is to track if the player has lost or gained HP
	int _MaxHealth;

	[SerializeField] Text _TotalPikminText;
	[SerializeField] Text _InSquadText;
	[SerializeField] Text _OnFieldText;
	[SerializeField] Text _DayText;

	Player _Player;
	PlayerPikminManager _PikminManager;

	void Start()
	{
		_Player = Player.player;
		_PikminManager = _Player.GetPikminManager();
		_MaxHealth = _Player.GetMaxHealth();
		_ReferenceHealth = _Player.GetHealth();
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
			
		if(_Player.GetHealth() < _ReferenceHealth) print("Ouch! I lost some HP!");
		_ReferenceHealth = _Player.GetHealth();

		float step = (float)_ReferenceHealth / (float)_MaxHealth;
		// Sets the fill amount to be a fraction (0 - dead, _MaxHealth - alive)
		// Newgame+ LD: I've also added a smooth movement to it like in Pikmin 1, and it uses a gradient now.
		_HealthCircle.fillAmount = Mathf.MoveTowards(_HealthCircle.fillAmount,step, Time.deltaTime);
		_HealthCircle.color = _HealthColor.Evaluate(step);
	}

	void SetPikminStats()
	{
		_TotalPikminText.text = PlayerStats._TotalPikmin.ToString();
		_DayText.text = PlayerStats._Day.ToString();
		_InSquadText.text = _PikminManager.GetSquadCount().ToString();
		_OnFieldText.text = _PikminManager.GetPikminOnField().ToString();
	}
}
