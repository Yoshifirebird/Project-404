/*
 * EveningCountdown.cs
 * Created by: Newgame+ LD
 * Created on: 4/5/2020 (dd/mm/yy)
 */

using UnityEngine;
using UnityEngine.UI;

public class EveningCountdown : MonoBehaviour
{
	public Gradient timeGradient;
	public Text header;

	public Text countdownTimer;
	public Text countdownTimerPulse;

	public float[] timestamps;
	public int timestampsLeft;

	void Start ()	{

		timestampsLeft = timestamps.Length;


	}


    // Update is called once per frame
    void Update()
    {

		float parameter = Mathf.InverseLerp(timestamps[timestamps.Length-1],timestamps[0], GameManager._DayController._CurrentTime);
		float tempAlpha = header.color.a;
		header.color = timeGradient.Evaluate(parameter);
		header.color = new Color(header.color.r,header.color.g,header.color.b,tempAlpha);

		if(GameManager._DayController._CurrentTime > timestamps[timestampsLeft-1] && timestampsLeft >= 0)	{


			timestampsLeft -= 1;
			countdownTimer.text = timestampsLeft.ToString();
			countdownTimerPulse.text = timestampsLeft.ToString();

			countdownTimer.gameObject.SetActive(false);
			countdownTimer.gameObject.SetActive(true);

		}
    }
}
