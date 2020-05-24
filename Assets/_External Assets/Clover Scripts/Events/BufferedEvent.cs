/*
 * BufferedEvent.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * This script is used to trigger a Unity event after a certain period of time
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BufferedEvent : MonoBehaviour {

	public UnityEvent onBufferDone;
	public bool triggerOnActive;
	public float triggerOnActiveTime;


	void OnEnable ()	{

		if(triggerOnActive)TriggerEventAfterTime (triggerOnActiveTime);
	}

	public void TriggerEventAfterTime (float delay) {

		StartCoroutine(execute (delay));
	
	}


	IEnumerator execute (float delay)	{

		yield return new WaitForSeconds (delay);
		onBufferDone.Invoke ();

	}

}
