﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeline : MonoBehaviour
{
	public RectTransform playhead;
	public Text debugTime;
	public AnimationCurve sunOpacity;
	public Image[] sunPics;
	public float[] opDiv;
	public AnimationCurve moonOpacity;
	public Image moonPic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float currentTime = GameManager._DayController._CurrentTime / DayManager._TotalDayTime;

		int i = 0;
		foreach(Image target in sunPics)	{
			
			target.color = new Color(target.color.r,target.color.g,target.color.b,sunOpacity.Evaluate(currentTime % 1)*opDiv[i]);
			i++;
		}

		moonPic.color = new Color(1,1,1,moonOpacity.Evaluate(currentTime % 1));

		playhead.anchorMin = new Vector2(currentTime  %1,0.5f);
		playhead.anchorMax = new Vector2(currentTime %1,0.5f);

		var td = TimeSpan.FromSeconds(GameManager._DayController._CurrentTime);
		if(debugTime) 
			debugTime.text = string.Format("{0:00}:{1:00}", td.Minutes, td.Seconds);
			
    }
}