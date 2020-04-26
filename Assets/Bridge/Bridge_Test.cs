/*
 * Bridge_Test.cs
 * Created by: Ambrosia
 * Created on: 26/4/2020 (dd/mm/yy)
 * Created for: Needing to test bridge generation
 */

using System.Collections.Generic;
using UnityEngine;

public class Bridge_Test : MonoBehaviour {
	[Header ("Components")]
	[SerializeField] Transform _StartPoint = null;
	[SerializeField] Transform _EndPoint = null;

	[Header ("Bridge Parts")]
	// Will be used to create the inital ramp, midpoints and ending ramp
	[SerializeField] GameObject _Piece = null;

	[Header ("Settings")]
	// The size of the steps we're going to take when iterating
	[SerializeField] float _StepSize = 1;
	// The angle (in degrees) that the starting and ending ramp will be instantiated at
	[SerializeField] float _AngleOfRamp = 25;

	// An added height that the midpoint will be instantiated at
	float _RampHeightOffset = 0;
	// Distance between the start point and the end point (X & Z)
	float _DistanceBetween = 0;
	// How many times we'll have to iterate before reaching the destination
	int _StepsToFinish = 0;
	// Stores all of the bridge pieces ([0] will be start ramp, [end index] will be end ramp)
	List<GameObject> _BridgePieces = new List<GameObject> ();

	private void Awake () {
		_DistanceBetween = Mathf.Sqrt (MathUtil.DistanceTo (_StartPoint.position, _EndPoint.position, false));
		_StepsToFinish = Mathf.CeilToInt (_DistanceBetween / _StepSize);

		// Height offset required for the ramp to be dynamically changing with the angle of the ramp
		_RampHeightOffset = (Mathf.Sin (_AngleOfRamp * Mathf.Deg2Rad) / 2) - (_Piece.transform.localScale.y / 2);

		print ($"DistanceBetween {_DistanceBetween}, StepsToFinish {_StepsToFinish} RampHeightOffset {_RampHeightOffset} AngleOfRamp {_AngleOfRamp}");

		// Stores the rotaiton to make an object look at the end point
		Quaternion lookAt = Quaternion.LookRotation ((_EndPoint.position - _StartPoint.position).normalized);
		// Spawn the starting ramp
		GameObject startingRamp = Instantiate (_Piece, _StartPoint.position + Vector3.up * _RampHeightOffset, Quaternion.identity);
		// Rotate the starting ramp to actually become a ramp, and look at the end point
		startingRamp.transform.rotation = Quaternion.Euler (lookAt.eulerAngles.x - _AngleOfRamp, lookAt.eulerAngles.y, lookAt.eulerAngles.z);
		// Add the starting ramp to the list of bridge pieces
		_BridgePieces.Add (startingRamp);
	}
}