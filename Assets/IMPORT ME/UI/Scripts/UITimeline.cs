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
		int i = 0;
		foreach(Image target in sunPics)	{
			
			target.color = new Color(target.color.r,target.color.g,target.color.b,sunOpacity.Evaluate(GameManager.timeOfDay%1)*opDiv[i]);
			i++;
		}

		moonPic.color = new Color(1,1,1,moonOpacity.Evaluate(GameManager.timeOfDay%1));

		playhead.anchorMin = new Vector2((GameManager.timeOfDay*2)%1,0.5f);
		playhead.anchorMax = new Vector2((GameManager.timeOfDay*2)%1,0.5f);

		var tod = GameManager.timeOfDay;
		int minute = Mathf.FloorToInt((tod*24)*60)%60;
		int hour = Mathf.FloorToInt((tod*24)+6)%24;



		if(debugTime) 
			debugTime.text = string.Format("{0:0}:{1:00}", hour, minute);
			
    }
}
