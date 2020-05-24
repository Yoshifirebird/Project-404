/*
 * AudioLoopSample.cs
 * Created by: ??? (Is from Gistix's Sonic '06 PC)
 * Created on: ??/??/???? (dd/mm/yy)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopSample : MonoBehaviour {

	public AudioSource source;
	public AudioClip clip;

	public int loopStart;
	public int loopEnd;

	// Use this for initialization
	void Start () {
		StartCoroutine (PlayAndLoopClip ());
	}
	
	IEnumerator PlayAndLoopClip () {
		source.clip = clip;	
		source.Play ();	

		while (true) {
			if (source.timeSamples > loopEnd)
				source.timeSamples = loopStart;
			
			yield return null;
		}	
	}
}
