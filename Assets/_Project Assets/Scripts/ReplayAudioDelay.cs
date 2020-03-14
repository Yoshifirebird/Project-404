﻿/*
 * ReplayAudioDelay.cs
 * Created by: Ambrosia
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class ReplayAudioDelay : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] AudioSource _ToReplay;

	[Header("Settings")]
	[SerializeField] float _Delay;
	[SerializeField] bool _UseRandomDelay = false;
	[SerializeField] Vector2 _RandomDelayBounds = Vector2.one * 2;

	float _CurrentDelay;
	float _CurrentTime = 0;

	void Awake()
	{
		// Set the current delay based on if we're using the random delay or not
		_CurrentDelay = _UseRandomDelay ? Random.Range(_RandomDelayBounds.x, _RandomDelayBounds.y) : _Delay;
	}

	void Update()
	{
		// Increment the timer
		_CurrentTime += Time.deltaTime;
		if (_CurrentTime >= _CurrentDelay)
		{
			// Play the audio, and reset the timer
			_ToReplay.Play();
			_CurrentTime = 0;

			// Change the Delay based on if we're using the random delay
			if (_UseRandomDelay)
			{
				_CurrentDelay = Random.Range(_RandomDelayBounds.x, _RandomDelayBounds.y);
			}
		}
	}
}
