/*
 * CustomInputModule.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Use an instance of this to get input using keycodes instead of the somewhat clunky input system.
 * (Unless you need to get axes. In which case... You'll have to set it up with a special setup)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class CustomInputModule : MonoBehaviour {

  public int playerIndex; //This is used for the TextAsset to load inputs

  public enum controlScheme { Generic, PS3Mac, PS3Win, PS4Mac, PS4Win, Xb360Mac, Xb360Win }
  public controlScheme m_controlScheme;

  [System.Serializable]
  public class btn {

    public string buttonName;
    public KeyCode input;
    public KeyCode negativeInput;
    public string axisTarget;

  }
  public List<btn> buttons;

  public TextAsset controlSettings;

  public void Start () {

    //if(isLocalPlayer)
    refreshInput ();

    //else this.enabled = false

  }

  public void refreshInput () {

    if (controlSettings != null) {
      var N = JSON.Parse (controlSettings.ToString ());

      buttons.Clear ();
      //print ("Cleared Buttons List");
      //print (N ["players"][playerIndex]["Buttons"][1]["buttonName"].Value);

      for (int i = 0; i < N["players"][playerIndex]["Buttons"].AsArray.Count; i++) {

        buttons.Add (new btn ());
        //print ("Button Added");

        //print ("Button Name Set! " + N["players"][playerIndex]["Buttons"][i]["buttonName"].Value);
        buttons[i].buttonName = N["players"][playerIndex]["Buttons"][i]["buttonName"].Value;

        buttons[i].axisTarget = N["players"][playerIndex]["Buttons"][i]["axisTarget"].Value;

        if (N["players"][playerIndex]["Buttons"][i]["input"].Value != "")
          buttons[i].input = (KeyCode) Enum.Parse (typeof (KeyCode), N["players"][playerIndex]["Buttons"][i]["input"].Value);
        //print ("Button Key Set! " + N["players"][playerIndex]["Buttons"][i]["input"].Value);
        if (N["players"][playerIndex]["Buttons"][i]["negativeInput"].Value != "")
          buttons[i].negativeInput = (KeyCode) Enum.Parse (typeof (KeyCode), N["players"][playerIndex]["Buttons"][i]["negativeInput"].Value);
        buttons[i].axisTarget = N["players"][playerIndex]["Buttons"][i]["axisTarget"].Value;
      }

    }

  }

  public bool GetButton (string target) {

    bool buttonFound = false;
    bool gottenButton = false;

    for (int i = 0; i < buttons.Count; i++) {

      if (buttons[i].buttonName == target) {
        buttonFound = true;
        if (!gottenButton) gottenButton = Input.GetKey (buttons[i].input);

      }

    }

    if (!buttonFound) Debug.LogError ("Input name " + target + " does not exist in the list...");
    return gottenButton;
  }

  public bool GetButtonDown (string target) {

    bool buttonFound = false;
    bool gottenButton = false;

    for (int i = 0; i < buttons.Count; i++) {

      if (buttons[i].buttonName == target) {
        buttonFound = true;
        if (!gottenButton) gottenButton = Input.GetKeyDown (buttons[i].input);

      }

    }

    if (!buttonFound) Debug.LogError ("Input name " + target + " does not exist in the list...");
    return gottenButton;
  }

  public bool GetButtonUp (string target) {

    bool buttonFound = false;
    bool gottenButton = false;

    for (int i = 0; i < buttons.Count; i++) {

      if (buttons[i].buttonName == target) {
        buttonFound = true;
        if (!gottenButton) gottenButton = Input.GetKeyUp (buttons[i].input);

      }

    }

    if (!buttonFound) Debug.LogError ("Input name " + target + " does not exist in the list...");
    return gottenButton;
  }

  public bool GetNegativeButton (string target) {

    bool buttonFound = false;
    bool gottenButton = false;

    for (int i = 0; i < buttons.Count; i++) {

      if (buttons[i].buttonName == target) {
        buttonFound = true;
        if (!gottenButton) gottenButton = Input.GetKey (buttons[i].negativeInput);

      }

    }

    if (!buttonFound) Debug.LogError ("Input name " + target + " does not exist in the list...");
    return gottenButton;
  }

  public bool GetNegativeButtonDown (string target) {

    bool buttonFound = false;
    bool gottenButton = false;

    for (int i = 0; i < buttons.Count; i++) {

      if (buttons[i].buttonName == target) {
        buttonFound = true;
        if (!gottenButton) gottenButton = Input.GetKeyDown (buttons[i].negativeInput);

      }

    }

    if (!buttonFound) Debug.LogError ("Input name " + target + " does not exist in the list...");
    return gottenButton;
  }

  public bool GetNegativeButtonUp (string target) {

    bool buttonFound = false;
    bool gottenButton = false;

    for (int i = 0; i < buttons.Count; i++) {

      if (buttons[i].buttonName == target) {
        buttonFound = true;

        if (!gottenButton) gottenButton = Input.GetKeyUp (buttons[i].negativeInput);

      }

    }

    if (!buttonFound) Debug.LogError ("Input name " + target + " does not exist in the list...");
    return gottenButton;
  }

  public float GetAxis (string target) {

    bool foundAxis = false;
    float acquiredValue = 0;

    for (int i = 0; i < buttons.Count; i++) {

      //print ("Target is " + target);
      //print ("We're looking at " + axes [i].axisName);

      if (buttons[i].buttonName == target) {

        foundAxis = true;

        //print ("Found " + target);

        if (buttons[i].axisTarget != "") {
          try {
            Input.GetAxis (buttons[i].axisTarget);
            acquiredValue += Input.GetAxis (buttons[i].axisTarget) + (GetButton (target) ? 1 :
              (GetNegativeButton (target) ? -1 : 0));

          }
          catch (UnityException) {
            Debug.LogError ("Axis name " + target + " has an invalid target!");
          }
        }
        acquiredValue += (GetButton (target) ? 1 :
          (GetNegativeButton (target) ? -1 : 0));

      }

    }

    if (!foundAxis) Debug.LogError ("Axis name " + target + " does not exist in the list...");

    //print (target + " acquires " + acquiredValue);
    acquiredValue = Mathf.Clamp (acquiredValue, -1, 1);
    return acquiredValue;
  }

  public float GetAxisRaw (string target) {

    bool foundAxis = false;
    float acquiredValue = 0;

    for (int i = 0; i < buttons.Count; i++) {

      //print ("Target is " + target);
      //print ("We're looking at " + axes [i].axisName);

      //print ("Found raw " + target);

      if (buttons[i].buttonName == target) {

        foundAxis = true;
        if (buttons[i].axisTarget != "") {

          try {
            Input.GetAxisRaw (buttons[i].axisTarget);
            acquiredValue += Input.GetAxis (buttons[i].axisTarget) + (GetButton (target) ? 1 :
              (GetNegativeButton (target) ? -1 : 0));

          }
          catch (UnityException) {
            Debug.LogError ("Axis name " + target + " has an invalid target!");
          }
        }

        acquiredValue += (GetButton (target) ? 1 :
          (GetNegativeButton (target) ? -1 : 0));
      }

    }

    if (!foundAxis) Debug.LogError ("Axis name " + target + " does not exist in the list...");

    //print ("Raw " + target + " acquires " + acquiredValue);
    acquiredValue = Mathf.Clamp (acquiredValue, -1, 1);
    return acquiredValue;
  }

}
