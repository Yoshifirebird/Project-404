/*
 * InputButtonEvent.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Events to play out when pressing a certain button with CustomInputModule
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputButtonEvent : MonoBehaviour {

	public CustomInputModule inputModule;

	[System.Serializable]
	public class menuButtons {

		public string input;

		public UnityEvent onButtonDown;
		public UnityEvent onButtonUp;
		public bool usable = true;
		public UnityEvent onButtonBlocked;

	}
	public List<menuButtons> m_menuButtons;

	// Update is called once per frame
	void Update () {

		foreach (menuButtons currentButton in m_menuButtons) {
			if (currentButton.usable) {
				if (inputModule.GetButtonDown (currentButton.input)) {
					currentButton.onButtonDown.Invoke ();
					print ("Get " + currentButton.input + " down!");
				}

				if (inputModule.GetButtonUp (currentButton.input))
					currentButton.onButtonUp.Invoke ();

			} else if (inputModule.GetButtonDown (currentButton.input))
				currentButton.onButtonBlocked.Invoke ();
		}

	}
}
