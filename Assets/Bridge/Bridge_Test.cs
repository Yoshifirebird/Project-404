/*
 * Bridge_Test.cs
 * Created by: Ambrosia
 * Created on: 26/4/2020 (dd/mm/yy)
 * Created for: Needing to test bridge generation
 */

using UnityEngine;

public class Bridge_Test : MonoBehaviour {
	[Header ("Components")]
	[SerializeField] Transform _StartPoint = null;
	[SerializeField] Transform _EndPoint = null;

	[Header ("Settings")]
	// The size of the steps we're going to take when iterating
	[SerializeField] float _StepSize = 1;

	// Distance between the start point and the end point (X & Z)
	float _DistanceBetween = 0;
	// How many times we'll have to iterate before reaching the destination
	int _StepsToFinish = 0;

	private void Awake () {
		_DistanceBetween = Mathf.Sqrt (MathUtil.DistanceTo (_StartPoint.position, _EndPoint.position, false));
		_StepsToFinish = Mathf.CeilToInt (_DistanceBetween / _StepSize);

		print ($"DistanceBetween {_DistanceBetween}, StepsToFinish {_StepsToFinish}");
	}
}