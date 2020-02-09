/*
 * WhistleController.cs
 * Created by: Ambrosia
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: controlling the whistle
 */

using UnityEngine;

public class WhistleController : MonoBehaviour
{
	Camera _Camera;

	[Header("Settings")]
	[SerializeField] LayerMask _GroundLayer;
	[SerializeField] float _YAxisOffset = 0.5f;
	
	private void Awake()
	{
		_Camera = Camera.main;
	}
	
	private void Update()
	{
		// Shoot a raycast from the mouse position for an infinite distance, filtering out any
		// hits other than objects that are on the _GroundLayer layer
		if (Physics.Raycast(_Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, _GroundLayer.value, QueryTriggerInteraction.Ignore))
		{
			// Move the whistle to the hit point
			Vector3 position = hit.point;
			position.y += _YAxisOffset;
			transform.position = position;

			// Rotate the whistle
			transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
		}
	}
}
