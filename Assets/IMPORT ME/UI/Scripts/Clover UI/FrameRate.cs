using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour
{
	public int avgFrameRate;
	public Text textfield;

	public bool accumulationMode;
	public int accumulation;
	public float timeLeft;

	void Update()
	{
		if (!accumulationMode) {
			float current = 0;
			current = Time.frameCount / Time.unscaledTime;
			avgFrameRate = (int)current;
			textfield.text = (avgFrameRate+ " FPS");

		} else {

			if (timeLeft > 0) {

				accumulation++;
				timeLeft -= Time.unscaledDeltaTime;

			} else {

				textfield.text = (accumulation + " FPS");
				timeLeft = 1;
				accumulation = 0;
			}



		}
	}


}