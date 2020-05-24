/*
 * SceneSwitcher.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Transition a scene with a loading screen!
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

	public float delay;
	public string destination;
	public bool switchAutomatically;

	// Use this for initialization
	void Start () {

		if(switchAutomatically) StartCoroutine (GO ());

	}

	IEnumerator GO () {

		yield return new WaitForSeconds (delay);

		switchScene (destination);


	}

	public SceneTransition loadScreen;

	public void switchScene (string destination)	{ //We use this function for use with Unity events...

		StartCoroutine (IswitchScene (destination,0.5f));	//Then we start the coroutine so we can use...

	}

	public void switchSceneDelayEvent (float delay)	{ //We use this function for use with Unity events...

		StartCoroutine (IswitchScene (destination, delay));	//Then we start the coroutine so we can use...

	}


	public IEnumerator IswitchScene (string destination, float delay)	{

		//print ("begin routine");
		yield return new WaitForSeconds (delay);		//these wait for seconds functions.

		if (loadScreen) {
			//print ("built loadscreen");
			SceneTransition loader = Instantiate (loadScreen);

			//print ("calling switch");
			loader.switchScene (destination);

		}
		else 
			SceneManager.LoadScene(destination);

	}


}
