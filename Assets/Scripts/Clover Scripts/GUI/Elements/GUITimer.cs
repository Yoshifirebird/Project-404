/*
This script uses one float to measure time in seconds, and another one
(if greater than zero) to measure its limit.
*/

using UnityEngine;
using UnityEngine.UI;

public class GUITimer : MonoBehaviour {

	public Text textField;
	private float timeCount;
	public float timeLimit;
	public float timeElapsed;


	public void Update () {

		timeCount = (timeLimit > 0) ? timeLimit - timeElapsed : timeElapsed;

		var minutes = Mathf.Floor (timeCount / 60); 					//Divide the guiTime by sixty to get the minutes.
		var seconds = Mathf.Floor (timeCount % 60);		//Use the euclidean division for the seconds.
		var fraction = (timeCount %1) * 99;

		//update the label value
		textField.text = string.Format ("{0:00}:{1:00}.{2:00}", minutes, seconds, fraction);
	}
}
