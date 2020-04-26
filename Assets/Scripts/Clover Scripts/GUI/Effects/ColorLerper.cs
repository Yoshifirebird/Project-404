using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorLerper : MonoBehaviour {

	[System.Serializable]
	public class option	{

		public AnimationCurve lerpCurve;
		public float duration;

		public Gradient colorGrad;

	}

	public option[] colorOptions;

	public Text textColoration;
	public Image imageColoration;
	public int index;


	// Use this for initialization
	void Awake () {

		textColoration = GetComponent<Text> ();
		imageColoration = GetComponent<Image> ();

	}

	// Update is called once per frame
	void Update () {

		//Color lerpFunction = Color.Lerp(colorOptions[colorOptionIndex].color1, colorOptions[colorOptionIndex].color2, colorLerpMod());

		if (textColoration != null)
			textColoration.color = colorOptions [index].colorGrad.Evaluate (colorLerpMod (index));

		if(imageColoration != null) imageColoration.color = colorOptions [index].colorGrad.Evaluate (colorLerpMod (index));

	}

	public float colorLerpMod (int number)	{


		return colorOptions [number].lerpCurve.Evaluate (Mathf.Repeat(Time.unscaledTime, colorOptions[number].duration) / colorOptions[number].duration);


	}

	public void switchIntex (int targetIndex)	{

		index = targetIndex;

	}
}
