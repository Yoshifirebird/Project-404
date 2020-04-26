using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppQuitter : MonoBehaviour
{
  

	public void quitGame (float delay)	{

		Invoke ("finishQuitGame", delay);
	}

	void finishQuitGame ()	{

		print ("Game is quit");
		Application.Quit ();
	}


}
