/*
 * TextBox.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Is fed the acquired text from the TextboxEvent script and displays it.
 */

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextBox : MonoBehaviour {

  public TextboxEvent user;
  public TextVertexAccess textmask;
  public Text textfield;
  //public Text debugText;
  public int position;

  [System.Serializable]
  public class textBank {
    [TextArea (3, 10)]
    public List<string> targettext;

    public Font font;
    public int fontsize;
    public float volume;
    public float pitch;
    //public float speed;

    //public UnityEvent ontextbegin;
    public float timetiltype;
  }
  public List<textBank> m_textBank;

  public int arrayposition;
  public int bankPosition;

  float typetimeleft;
  public List<AudioClip> talksound;
  public AudioSource source;
  public bool interrupt;
  public bool spaceout;
  public int charspertype = 1;
  public bool nosoundrepeat;
  int lastrando;
  public UnityEvent OnTextEnd;
  public UnityEvent OnTextBegin;
  public bool istyping;
  public UnityEvent OnComplete;

  public bool forceStart;

  // Use this for initialization
  public void Begin () {

    istyping = true;

    if (bankPosition <= m_textBank.Count) {
      //m_textBank [bankPosition].ontextbegin.Invoke ();

      OnTextBegin.Invoke ();

      if (m_textBank[bankPosition].font != null)
        textfield.font = m_textBank[bankPosition].font;
      if (m_textBank[bankPosition].fontsize != 0) {
        textfield.fontSize = m_textBank[bankPosition].fontsize;
        textmask.textField.fontSize = m_textBank[bankPosition].fontsize;
      }
      if (source != null) {
        if (m_textBank[bankPosition].volume != 0)
          source.volume = m_textBank[bankPosition].volume;
        if (m_textBank[bankPosition].pitch != 0)
          source.pitch = m_textBank[bankPosition].pitch;
      }
    }

  }

  // Update is called once per frame
  void Update () {

    //arrayposition = Mathf.Clamp (arrayposition, 0, m_textBank.Count); //Make sure the array index doesn't exceed the limit!

    string targetText = m_textBank[bankPosition].targettext[arrayposition];

    if (position < targetText.Length) { //If we're not finished typing...

      if (typetimeleft > 0) { //If we still have time before typing a character...
        typetimeleft -= Time.deltaTime;

      }

      if (typetimeleft <= 0 && position > 0) { //If time is up... (We only do this when position is greater than zero so that the substring calculation doesn't fail
        //print (targettext [arrayposition].Substring (position - 1, 1));
        if (talksound.Count > 0)
          //print ("INF INSPECT A3");
          if ((spaceout && m_textBank[bankPosition].targettext[arrayposition].Substring (position - 1, 1) != " ") || !spaceout) { //If we either don't find a space in the next character or we flat out don't ignore spaces altogether...

            //print ("INF INSPECT A4");
            if (interrupt)
              source.Stop (); //Stop playing the sound if we're not interrupting...

            //print ("INF INSPECT A5");
            int randomchoice = Random.Range (0, talksound.Count);

            //print ("INF INSPECT A6");
            while (randomchoice == lastrando && nosoundrepeat) { randomchoice = Random.Range (0, talksound.Count); }

            //print ("INF INSPECT A7");
            source.PlayOneShot (talksound[randomchoice]);
            lastrando = randomchoice;

          }

      }

      //print ("INF INSPECT A8");
      while (typetimeleft <= 0 && istyping) { //This one keeps executing until time is above zero so that we're not bound by framerate

        int charsToType = charspertype;
        while (charsToType > 0) {

          bool tagSkip;
          if (m_textBank[bankPosition].targettext[arrayposition].Substring (position, 1) == "<") {
            tagSkip = true;
            print ("Begin skip!");
            while (tagSkip) {
              position++;
              print ("Skipping characters!");
              if (position < m_textBank[bankPosition].targettext[arrayposition].Length &&
                m_textBank[bankPosition].targettext[arrayposition].Substring (position, 1) == ">") {
                position += 1;
                if (position < m_textBank[bankPosition].targettext[arrayposition].Length && m_textBank[bankPosition].targettext[arrayposition].Substring (position, 1) != "<")
                  tagSkip = false;
              }
              else if (position >= m_textBank[bankPosition].targettext[arrayposition].Length)
                tagSkip = false;
            }

          }

          //print ("INF INSPECT A9");
          position += 1;
          charsToType -= 1;

        }
        typetimeleft += m_textBank[bankPosition].timetiltype;
      }

    }

    //print ("INF Check 2");

    if (istyping) {
      position = Mathf.Clamp (position, 0, m_textBank[bankPosition].targettext[arrayposition].Length);

      //textmask.changeMask ();
      textmask.textField.color = Color.red;
      //print (getTaglessText (m_textBank[bankPosition].targettext));
      textfield.text = m_textBank[bankPosition].targettext[arrayposition]; //Set the text!
      textmask.textField.text = m_textBank[bankPosition].targettext[arrayposition];
      textmask.vertIndex = Mathf.RoundToInt (position * 4.0f);
      //debugText.text = m_textBank [bankPosition].targettext[arrayposition].Substring (0, position);
    }

    //print ("INF Check 3");

    if (position >= targetText.Length && istyping) {

      OnTextEnd.Invoke ();
      istyping = false;

    }

    //print ("INF Check 4");

    if (Input.GetKeyDown (KeyCode.Space)) { //Advance the text and reset the position when we hit space

      if (position >= m_textBank[bankPosition].targettext[arrayposition].Length) {

        position = 0;
        arrayposition += 1;

        if (arrayposition >= m_textBank[bankPosition].targettext.Count) {

          bankPosition++;
          arrayposition = 0;
        }

        if (bankPosition < m_textBank.Count) {

          //print ("Text next! at " + arrayposition);
          istyping = true;

          //m_textBank[bankPosition].ontextbegin.Invoke ();
          if (m_textBank[bankPosition].font != null)
            textfield.font = m_textBank[bankPosition].font;
          if (m_textBank[bankPosition].fontsize != 0) {
            textfield.fontSize = m_textBank[bankPosition].fontsize;
            textmask.textField.fontSize = m_textBank[bankPosition].fontsize;
          }
          if (source != null) {
            if (m_textBank[bankPosition].volume != 0)
              source.volume = m_textBank[bankPosition].volume;
            if (m_textBank[bankPosition].pitch != 0)
              source.pitch = m_textBank[bankPosition].pitch;
          }

          OnTextBegin.Invoke ();
        }
        else {

          //print ("All done!");
          OnComplete.Invoke ();
          user.m_textEvents.OnTextClose.Invoke ();
          user.disengage ();
          //return;
          //textmask.transform.localScale = new Vector3 (1, 1, 1);
        }

      }
      else {

        position = m_textBank[bankPosition].targettext[arrayposition].Length;
      }

    }

    //print ("ALL CLEAR");

    if (forceStart) {

      Begin ();
      forceStart = false;

    }

  }

}
