/*
 * Plant.cs
 * Created by: Ambrosia
 * Created on: 10/4/2020 (dd/mm/yy)
 * Created for: needing plants to be animated
 */

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource), typeof(Animator))]
public class Plant : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] AudioClip _InteractSound;
	SphereCollider _Collider;
	AudioSource _Audio;
	Animator _Animator;

	[Header("Settings")]
	// The prepended I expands to 'Interaction'
	[SerializeField] LayerMask _ILayers;
	[SerializeField] Vector3 _IOffset = Vector3.forward;
	[SerializeField] float _IForwardOffset;
	[SerializeField] float _IRadius = 2.5f;

	List<Collider> _Interacting = new List<Collider>();

	void Awake()
	{
		// Set the collider radius to the interaction radius, if theres a mismatch between the two in scene this'll fix it
		_Collider = GetComponent<SphereCollider>();
		_Collider.radius = _IRadius;
		_Collider.center = _IOffset + transform.forward * _IForwardOffset;

		_Animator = GetComponent<Animator>();
		_Audio = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		// Check if the layer is within the interacting layers
		int layer = other.gameObject.layer;
		if (_ILayers != (_ILayers | (1 << layer)))
			return;

		_Interacting.Add(other);

		_Animator.SetBool("Shake", true);
	}

	void OnTriggerExit(Collider other)
	{
		// Check if the layer is within the interacting layers
		int layer = other.gameObject.layer;
		if (_ILayers != (_ILayers | (1 << layer)))
			return;

		_Interacting.Remove(other);

		if (_Interacting.Count == 0)
			_Animator.SetBool("Shake", false);
	}

	public void PlayShakeAudio()
	{
		_Audio.clip = _InteractSound;
		_Audio.Play();
	}
}
