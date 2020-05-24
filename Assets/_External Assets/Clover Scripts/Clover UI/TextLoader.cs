/*
 * TextLoader.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * An instance of this script and just the script alone through various functions can be used to retrieve text from a JSON file
 * via SimpleJSON. Just make sure a valid language is set.
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class TextLoader : MonoBehaviour {

	public string filePath;

	public Text targetText;

	public bool activateOnPlay = true;


	// Use this for initialization
	void Start () {

		targetText = GetComponent<Text> ();
		if(activateOnPlay) targetText.text = getText (filePath, targetText.text);


	}
		

	public static string getText (string path, string box)	{

		//print ("Lang access " + accessLang ());
		TextAsset jsonFinder = Resources.Load("Text/" + accessLang() + "/" + path+"."+accessLang()) as TextAsset;

		//print ("Text/" + accessLang () + "/" + path + ".json");

		//print(jsonFinder.ToString ());
		if (jsonFinder != null) {
			var N = JSON.Parse (jsonFinder.ToString ());
			var textString = N [box][0].Value != null ? N [box][0].Value : path; 
			return textString;
		} else
			return ("This message should not appear! If it does, that means the text file is missing!");

		//print (textString);


	}

	public static List<string> getTextBank (string path, string box)	{


		TextAsset jsonFinder = Resources.Load("Text/" + accessLang() + "/" + path+"."+accessLang()) as TextAsset;

		if (jsonFinder != null) {
			var N = JSON.Parse (jsonFinder.ToString ());

			List<string> textArray = new List<string> ();

			print ("Bank array count is " + N [box].Count);

			for (int i = 0; i < N [box].Count; i++) {


				textArray.Add (N[box][i].Value != null ? N[box][i].Value : path);
				print (N [box] [i].Value);
			}

			//var textString = N [box][0].Value != null ? N [box][0].Value : path; 


			return textArray;
		} else {

			List<string> errorEntry = new List<string> ();

			errorEntry.Add ("The document is missing!");
			errorEntry.Add ("Fix it!");

			return (errorEntry);

		}
			

		//print (textString);


	}

	public void getTextEvent (string box)	{

		TextAsset jsonFinder = Resources.Load("Text/" + accessLang() + "/" + filePath+"."+accessLang()) as TextAsset;

		var N = JSON.Parse(jsonFinder.ToString ());
		var textString = N [box][0].Value != null ? N [box][0].Value : filePath; 

		//print (textString + " Called from event");

		//targetText.text = textString;
		targetText.text = textString;

	}

	public void changePath (string path)	{

		filePath = path;

	}

	public static string accessLang ()	{

		return GameManager._Language.ToString();

	}
			
}
