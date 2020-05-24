/*
 * UITimeline.cs
 * Created by: Newgame+ LD
 * Created on: 6/5/2020 (dd/mm/yy)
 */

using System;
using UnityEngine;
using UnityEngine.UI;

public class UITimeline : MonoBehaviour {
  public RectTransform playhead;
  public Text debugTime;
  public AnimationCurve sunOpacity;
  public Image[] sunPics;
  public float[] opDiv; //Opacity division in case you're wondering.
  public AnimationCurve moonOpacity;
  public Image moonPic;

  void Update () {
    float currentTime = GameManager._DayController._CurrentTime;

    for (int i = 0; i < sunPics.Length; i++) {
      sunPics[i].color = new Color (sunPics[i].color.r, sunPics[i].color.g, sunPics[i].color.b, sunOpacity.Evaluate (currentTime % 1) * opDiv[i]);
    }

    moonPic.color = new Color (1, 1, 1, moonOpacity.Evaluate (currentTime % 1));

    playhead.anchorMin = playhead.anchorMax = new Vector2 (0, 0.5f);

    var td = TimeSpan.FromSeconds (((GameManager._DayController._CurrentTime + 0.25) % 1) * 1440);
    if (debugTime) {
      debugTime.text = string.Format ("{0:0}:{1:00}", td.Minutes, td.Seconds);
    }

  }
}
