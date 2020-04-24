/*
 * Onion.cs
 * Created by: Ambrosia
 * Created on: 12/4/2020 (dd/mm/yy)
 * Created for: Needing an object to store our Pikmin in
 */

using UnityEngine;

public class Onion : MonoBehaviour
{
	public enum OnionType
	{
		Classic, // When first finding an onion, it will be this
		Master	 // Main onion that has the combination of other onions
	}

	//[Header("Components")]


	[Header("Settings")]
	[SerializeField] OnionType _Type;
	[SerializeField] Colour _Colour;

	PlayerStats.PikminStats _Stats;
	bool _CanUse = false;

	void Update()
	{
		_Stats = PlayerStats.GetStats(_Colour);

		/*if (_CanUse && Input.GetButtonDown())
		{

		}*/
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_CanUse = true;
		}
	}
}
