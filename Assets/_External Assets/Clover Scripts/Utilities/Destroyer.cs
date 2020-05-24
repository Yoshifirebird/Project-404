/*
 * Destroyer.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Destroy an object after some time. Primarily for instantiated particle systems
 */

using UnityEngine;

public class Destroyer : MonoBehaviour {

	public float timeLeft = 2;
	public bool unscaled;

	void Update () {

		if (timeLeft > 0) {

			timeLeft -= unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
		} else {


			Destroy (gameObject);
		}

	}

	public void destroyNow ()	{

		Destroy (gameObject);

	}
}
