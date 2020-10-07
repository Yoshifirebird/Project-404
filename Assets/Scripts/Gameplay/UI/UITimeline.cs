/*
 * UITimeline.cs
 * Created by: Newgame+ LD
 * Created on: 6/5/2020 (dd/mm/yy)
 */

using System;
using UnityEngine;
using UnityEngine.UI;

public class UITimeline : MonoBehaviour {
  [SerializeField] RectTransform playhead = null;
  [SerializeField] Text debugTime = null;
  [SerializeField] AnimationCurve sunOpacity = null;
  [SerializeField] CanvasGroup sunPic = null;
  [SerializeField] AnimationCurve moonOpacity = null;
  [SerializeField] CanvasGroup moonPic = null;

  void Update () {
    if (GameManager._IsPaused || GameManager._Player._Paralyzed)
    {
      return;
    }

    float currentTime = GameManager._DayController._CurrentTime;

		sunPic.alpha = sunOpacity.Evaluate(currentTime%1);

		moonPic.alpha = moonOpacity.Evaluate(currentTime%1);

    playhead.anchorMin = playhead.anchorMax = new Vector2 (currentTime * 2 % 1, 0.5f);

    if (debugTime) {
      TimeSpan td = TimeSpan.FromSeconds (((GameManager._DayController._CurrentTime + 0.25) % 1) * 1440);
      debugTime.text = string.Format ("{0:0}:{1:00}", td.Minutes, td.Seconds);
    }
  }
}
