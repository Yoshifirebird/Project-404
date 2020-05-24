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
	public bool changeColor = true;
	public bool changeOpacity = true;


	// Use this for initialization
	void Awake () {

		textColoration = GetComponent<Text> ();
		imageColoration = GetComponent<Image> ();

	}

	// Update is called once per frame
	void Update () {


		Color targetColor = colorOptions [index].colorGrad.Evaluate (colorLerpMod (index));

		if (textColoration != null)	{

			var tempText = textColoration.color;

				if(changeColor){

					tempText.r = targetColor.r;
					tempText.g = targetColor.g;
					tempText.b = targetColor.b;
				}

				if(changeOpacity)
				{

					tempText.a = targetColor.a;
				}
			textColoration.color = tempText;
		}
			

		if(imageColoration != null) {

			//imageColoration.color = targetColor;
			var tempImage = imageColoration.color;

				if(changeColor){

					tempImage.r = targetColor.r;
					tempImage.g = targetColor.g;
					tempImage.b = targetColor.b;
				}

				if(changeOpacity)
				{

					tempImage.a = targetColor.a;
				}

			imageColoration.color = tempImage;
		}

	}

	public float colorLerpMod (int number)	{


		return colorOptions [number].lerpCurve.Evaluate (Mathf.Repeat(Time.unscaledTime, colorOptions[number].duration) / colorOptions[number].duration);


	}

	public void switchIntex (int targetIndex)	{

		index = targetIndex;

	}
}
