/*
 * DayManager.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using System;
using UnityEngine;

// TODO: stubbed atm, expand upon this
public class DayManager : MonoBehaviour
{
	[SerializeField] int _LengthOfDay = 50;
	[SerializeField] string _LengthOfDayMMSS = "00:00";

	void Awake()
	{
		GameManager._DayManager = this;
	}

	void OnDrawGizmosSelected()
	{
		var ts = TimeSpan.FromSeconds(_LengthOfDay);
		_LengthOfDayMMSS = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
	}
}
