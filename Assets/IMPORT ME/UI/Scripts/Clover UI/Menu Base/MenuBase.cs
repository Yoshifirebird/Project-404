//This script contains all the menu stuff

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuBase : CommonBase {

	public bool enterAutomatically = true;

	public UIButtonBase highlighted;

	public UnityEvent onAdjust;
	public UnityEvent onCancel;
	public UnityEvent onEnterMenu;
	public UnityEvent onExitMenu;

	public RectTransform selector;
	public float selectorRate = 1;

	[Header("Input")]
	public bool canInput = true;
	public Vector2 inputAxes;
	private float inputDelay = 0.325f;
	private float inputDelayDefault = 0.325f;
	[Space(10)]
	public bool inputSubmit;
	public bool canSubmit = true;
	[Space(10)]
	public bool inputCancel;
	public bool canCancel = true;

	//Add a custom input measurer



	public new void Start ()	{



		base.Start ();

	
		//menuManager = GameObject.FindWithTag ("GameController").transform.GetComponent<MenuManager>();

		moveSelector (selectorRate);
		if(enterAutomatically)enterMenu ();

		//if(m_animator) m_animator.SetBool ("InMenu", true);


	}

	public new void Update ()	{
		if(canInput) ManageInputs ();
	}

	public void ManageInputs ()	{

		if (inputAxes.magnitude < 0.1f) {

			inputDelay = 0;

		} else if (inputDelay <= 0) {

			inputDelay = inputDelayDefault;

			if (inputAxes.y > 0.1f && Mathf.Abs (inputAxes.x) < Mathf.Abs (inputAxes.y) && highlighted.upButton != null) {
				//print ("Menu Input UP");
				highlighted.exitEvent ();
				highlighted.upButton.enterEvent ();

				//print ("Menu Input UP");
				onAdjust.Invoke ();
			} else if (inputAxes.y < -0.1f && Mathf.Abs (inputAxes.x) < Mathf.Abs (inputAxes.y) && highlighted.downButton != null) {
				//print ("Menu Input DOWN");
				highlighted.exitEvent ();
				highlighted.downButton.enterEvent ();

				onAdjust.Invoke ();
			}
			else if (inputAxes.x < -0.1f && Mathf.Abs (inputAxes.x) > Mathf.Abs (inputAxes.y) && highlighted.leftButton != null) {
				//print ("Menu Input LEFT");
				highlighted.exitEvent ();
				highlighted.leftButton.enterEvent ();

				onAdjust.Invoke ();
			}
			else if (inputAxes.x > 0.1f && Mathf.Abs (inputAxes.x) > Mathf.Abs (inputAxes.y) && highlighted.rightButton != null) {
				//print ("Menu Input RIGHT");
				highlighted.exitEvent ();
				highlighted.rightButton.enterEvent ();

				onAdjust.Invoke ();
			}
			moveSelector (selectorRate);


		} else {

			inputDelay -= Time.unscaledDeltaTime;

		}

		if (inputSubmit) {

			if(canSubmit) submitPress ();
			inputSubmit = false;

		}

		if (inputCancel) {

			if (canCancel) {
				cancelPress ();

			}
			inputCancel = false;

		}


	}


	public virtual void submitPress ()	{

		highlighted.clickEvent ();

	}

	public virtual void cancelPress ()	{

		onCancel.Invoke ();


	}

	public void enterMenu () 	{

		onEnterMenu.Invoke ();
		//print ("Invoker alert");
		if(highlighted)highlighted.enterEvent();
		print ("Menu Enter!");

	}

	public void exitMenu () 	{


		//if(m_animator)m_animator.SetTrigger ("ExitMenu");

		inputControl (false);

		onExitMenu.Invoke ();

		print ("Menu Exit!");
	}
		

	public void moveSelector (float rate)	{	//Moving the selector around

		StopCoroutine ("selectorMovement");
		if(selector) StartCoroutine ("selectorMovement", rate);

	}

	public IEnumerator selectorMovement (float rate)	{		

		//if (highlighted)
			//print ("Prepare to move selector");
		//	else
			//print ("There's no selector targeted!");
		RectTransform buttonTarget = highlighted.GetComponent<RectTransform> ();
		Vector2 targetSize = new Vector2 (buttonTarget.sizeDelta.x, buttonTarget.sizeDelta.y);

		while (selector.position != buttonTarget.position || selector.sizeDelta != targetSize || selector.pivot != buttonTarget.pivot) {

			//print ("SE" + selector.position);
			//print ("BU" + buttonTarget.position);

			selector.sizeDelta = new Vector2 (Mathf.MoveTowards(selector.sizeDelta.x, targetSize.x, (Time.unscaledDeltaTime*(rate*60))*(Mathf.Abs(selector.sizeDelta.x-targetSize.x)/2)),
				Mathf.MoveTowards(selector.sizeDelta.y,targetSize.y, (Time.unscaledDeltaTime*(rate*60))*(Mathf.Abs(selector.sizeDelta.y-targetSize.y)/2)));
			
			selector.position = new Vector3 (Mathf.MoveTowards (selector.position.x, buttonTarget.position.x, (Time.unscaledDeltaTime*(rate*60))*(Mathf.Abs (selector.position.x-buttonTarget.position.x)/2)),
				Mathf.MoveTowards (selector.position.y, buttonTarget.position.y, (Time.unscaledDeltaTime*(rate*60))*(Mathf.Abs (selector.position.y-buttonTarget.position.y)/2)), buttonTarget.position.z);

			selector.pivot = buttonTarget.pivot;
			

			yield return new WaitForEndOfFrame ();
		}


	}

	public void menuTransition (MenuBase target)	{

		Instantiate (target.transform, transform.parent);

		lockInput();

	}

	public MenuBase menuTransitionData (MenuBase target)	{

		MenuBase newMenu = Instantiate (target.transform, transform.parent).GetComponent<MenuBase>();

		return newMenu;

	}




	public void selectorShow (bool shown)	{
		//print ("Change selector");


		selector.gameObject.SetActive (shown);
		if (shown == true) {
			moveSelector (selectorRate);
			//print ("GO");
		}
		else {
			//print ("STOP");
			StopCoroutine ("selectorMovement");
		}

	}

	public void inputControl (bool unlocked)	{

		//Debug.LogError ("INPUTS CHANGED");
		canInput = unlocked;



	}

	public void lockInput ()	{

		//Debug.LogError ("INPUTS LOCKED");
		canInput = false;

	}


	public void unlockInput ()	{


		canInput = true;

	}
}