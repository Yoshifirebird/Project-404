using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.Experimental.PlayerLoop;

public class UIButtonBase : CommonBase, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	public MenuBase master;
	public bool interactable = true;
	public bool canHighlight = true;
	public bool highlighted;
	public bool isClicked;

	public UnityEvent OnEnter;
	public UnityEvent OnHighlight;
	public UnityEvent OnExit;
	public UnityEvent OnClick;
	public UnityEvent OnClickRelease;
	public UnityEvent OnClickBlocked;

	public UIButtonBase upButton;
	public UIButtonBase downButton;
	public UIButtonBase leftButton;
	public UIButtonBase rightButton;

	public void Update ()	{

		if(m_animator)m_animator.SetBool ("Highlighted",highlighted);

	}

	public void rehighlight ()	{ //Highlight this one after un-highlighting something else

		//print ("Initiate Rehighlight");
		if (master) {
			master.highlighted.setHighlight (false);

			//print ("Rehighlighted");
			master.highlighted = this;

			master.selectorMovement (master.selectorRate);
		
		} else
			print ("No master detected. This function will be ignored");
	}


	public void setHighlight (bool settohl)	{ //Set the button's highlight status

		highlighted = settohl;

	}

	public void OnPointerEnter(PointerEventData eventData)	{  //When cursor enters...

		enterEvent ();
		if(master && master.canInput && interactable) master.onAdjust.Invoke ();

	}

	public void enterEvent()	{

		rehighlight ();

		//print (gameObject.name + " Button Enter");

		//Is it interactable?

			if (interactable) OnEnter.Invoke (); 		//Call some events for entering

		if (interactable || canHighlight) OnHighlightEvent (); 	//Check for highlighting


	}

	public void OnPointerExit(PointerEventData eventData)	{

		exitEvent ();
		isClicked = false;
	}

	public void OnPointerDown(PointerEventData eventData)	{

		clickEvent ();
	}

	public void OnPointerUp(PointerEventData eventData)	{

		print ("Pointer up!");
		 
		if (isClicked) {
			clickEventRelease ();
			isClicked = false;
		}
	}

	public void OnHighlightEvent ()	{

		//print ("Highlight event A?");

		if (!highlighted) {		//Are we not already highlighted

			//print ("Highlight event B?");

			highlighted = true;



			if (master != null)
				master.moveSelector (master.selectorRate); 	//If connected to a menu base, unhighlight the other buttons

			//print ("Highlight event C?");

			OnHighlight.Invoke (); 	//Call some events for highlight
		} else {
			print ("Uh oh. Something made me already highlight.");
		}

	}



	public void exitEvent()	{

		//print (gameObject.name + " Button Exit");

		if(interactable) OnExit.Invoke ();	//Call some events for exiting
		isClicked = false;
		highlighted = false;

	}



	public void clickEvent()	{

		if (interactable) {
			OnClick.Invoke (); //Call some events for clicking
			isClicked = true;
		}

		else
			OnClickBlocked.Invoke ();

	}

	public void clickEventRelease()	{

		if (interactable) {
			OnClickRelease.Invoke (); //Call some events for clicking
			isClicked = false;
		}

	}


	public void setInteractible (bool state)	{	//Set our interactability to the pointer's state

		interactable = state;
	}

	public void setSelectedToThis ()	{


		rehighlight ();
		master.moveSelector (9999);
	}

}
