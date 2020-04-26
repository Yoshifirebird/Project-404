using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class RandomSound : MonoBehaviour {

	public AudioClip[] sounds;

	// Use this for initialization
	void OnEnable () {

		GetComponent<AudioSource> ().PlayOneShot (sounds [Random.Range (0, sounds.Length)]);

	}

}