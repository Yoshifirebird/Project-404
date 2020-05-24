/*
 * SceneTransition.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Loading screen
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransition : CommonBase {

	public GameObject errorScreen;
	public UnityEvent onComplete;
	public UnityEvent onFail;

	public void switchScene (string destinationScene)	{

		StartCoroutine(LoadYourAsyncScene (destinationScene));

	}


	IEnumerator LoadYourAsyncScene (string destinationScene)
	{
		yield return new WaitForSeconds (1);
		// The Application loads the Scene in the background as the current Scene runs.

		if (!Application.CanStreamedLevelBeLoaded (destinationScene)) {

			print (destinationScene + " is not on the list!!");
			//errorScreen.SetActive (true);
			loadError ();
			yield break;
		}

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene);



		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		onComplete.Invoke();
		//m_animator.SetTrigger ("Exit");
		delete (5);
	}

	public virtual void loadError()	{

		onFail.Invoke();
	}




}
