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

	Player _Player;
	int _MaxHealth;

	//[Header("Settings")]
	
	
	private void Awake()
	{
		_Player = GetComponent<Player>();
		_MaxHealth = _Player.GetMaxHealth();
	}
	
	private void Update()
	{
		SetHealthCircle();

		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			_Player.TakeHealth(10);
		}
	}
	
	private void SetHealthCircle()
	{
		float step = (float)_Player.GetHealth() / (float)_MaxHealth;
		// Sets the fill amount to be a fraction (0 - dead, _MaxHealth - alive)
		_HealthCircle.fillAmount = step;
		_HealthCircle.color = Color.Lerp(Color.red, Color.green, step);
	}
}
