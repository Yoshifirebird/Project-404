/*
 * Onion.cs
 * Created by: Ambrosia
 * Created on: 12/4/2020 (dd/mm/yy)
 * Created for: Needing an object to store our Pikmin in
 */

using UnityEngine;

public class Onion : MonoBehaviour
{
	//[Header("Components")]


	[Header("Settings")]
	[SerializeField]
	Colour _OnionColour;

	PlayerStats.PikminStats _Stats;
	bool _CanUse = false;

	void Update()
	{
		_Stats = PlayerStats.GetStats(_OnionColour);

		/*if (_CanUse && Input.GetButtonDown())
		{

		}*/
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{

		}
	}
}
