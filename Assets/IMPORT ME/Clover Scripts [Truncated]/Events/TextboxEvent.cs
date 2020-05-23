using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class TextboxEvent : MonoBehaviour {

	public TextBox boxTarget;


	//public string headerPath;
	//public string header;
	public string bodyPath;
	public bool active;

	[System.Serializable]
	public class textArray {

		public string text;
		public Font font;
		public int fontsize=24;
		public float volume=1;
		public float pitch=1;
		public float timetiltype=0.125f;
		public UnityEvent OnThisTextStartTyping;
		public UnityEvent OnThisTextType;
		public UnityEvent OnThisTextTypeIgnoreSpace;
		public UnityEvent OnThisTextEnd;
		public UnityEvent OnThisTextAdvance;
	}

	public List<textArray> m_textArray;

	[System.Serializable]
	public class textEvents {
		public UnityEvent OnTextInitialize;
		public UnityEvent OnTextStartTyping;
		public UnityEvent OnTextType;
		public UnityEvent OnTextTypeIgnoreSpace;
		public UnityEvent OnTextEnd;
		public UnityEvent OnTextAdvance;
		public UnityEvent OnTextClose;
	}
	public textEvents m_textEvents;

	public void initiate () 	{

		if (!active)
			return;

		//active = false;
		//print ("Initiate " + m_textArray [0].text);

		TextBox current = Instantiate (boxTarget, boxTarget.transform.position, boxTarget.transform.rotation).GetComponent<TextBox> ();

		current.user = this;

		//print (current.m_textBank [0].targettext);

		current.m_textBank = new List<TextBox.textBank> (new TextBox.textBank[0]);

		//print (current.m_textBank.Count);


		for (int i = 0; i < m_textArray.Count; i++) {

			//print (current.m_textBank[i].targettext);

			current.m_textBank.Add (new TextBox.textBank());

			current.m_textBank[i].targettext = TextLoader.getTextBank (bodyPath, m_textArray [i].text);

			current.m_textBank [i].volume = m_textArray [i].volume;
			current.m_textBank [i].pitch = m_textArray [i].pitch;
			current.m_textBank [i].fontsize = m_textArray [i].fontsize;
			current.m_textBank [i].timetiltype = m_textArray [i].timetiltype;
		}

		current.Begin ();

		//current.headerText.text = TextLoader.getText (headerPath, header);
	}

	public void disengage ()	{

		StartCoroutine (holdforbit ());

	}

	public IEnumerator holdforbit ()	{

		active = false;
		yield return new WaitForSeconds (0.125f);
		active = true;
	}

}
