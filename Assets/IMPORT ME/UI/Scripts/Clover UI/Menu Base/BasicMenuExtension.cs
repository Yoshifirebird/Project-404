using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicMenuExtension : CommonBase {

	public CustomInputModule inputModule;
	public MenuBase menu;

	[System.Serializable]
	public class menuButtons {

		public string input;

		public UnityEvent onButtonDown;
		public UnityEvent onButtonUp;
		public UnityEvent onNegativeButtonDown;
		public UnityEvent onNegativeButtonUp;
		public bool usable = true;
		public UnityEvent onButtonBlocked;

	}
	public List<menuButtons> m_menuButtons;


	// Use this for initialization
	public new void Start () {


		//inputModule = GameObject.FindWithTag ("GameInput").GetComponent<CustomInputModule> ();
		//print ("INPUT OK");
	}

	// Update is called once per frame
	public new void Update () {

		if (menu.canInput) {
			foreach (menuButtons currentButton in m_menuButtons) {
				if (currentButton.usable) {


					if (inputModule.GetButtonDown (currentButton.input)) {
						currentButton.onButtonDown.Invoke ();
						//print ("Get " + currentButton.input + " down!");
					}

					if (inputModule.GetButtonUp (currentButton.input))
						currentButton.onButtonUp.Invoke ();

					if (inputModule.GetNegativeButtonDown (currentButton.input)) {
						currentButton.onNegativeButtonDown.Invoke ();
						//print ("Get " + currentButton.input + " down!");
					}

					if (inputModule.GetNegativeButtonUp (currentButton.input))
						currentButton.onNegativeButtonUp.Invoke ();

				} else if (inputModule.GetButtonDown (currentButton.input))
					currentButton.onButtonBlocked.Invoke ();
			}
		}

		//print("AxisGet " + inputModule.GetAxis ("Horizontal"));

		menu.inputAxes.x = inputModule.GetAxis ("Horizontal");
		menu.inputAxes.y = inputModule.GetAxis ("Vertical");

		menu.inputSubmit = inputModule.GetButtonDown ("Menu Confirm");
		menu.inputCancel = inputModule.GetButtonDown ("Menu Cancel");
	

	}
}
