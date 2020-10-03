/*
 * OnionMenu.cs
 * Created by: Newgame+ LD
 * Created on: 9/5/2020 (dd/mm/yy)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnionMenu : MonoBehaviour
{

	public OnionController master;

	public int[] pikminToDispense; //Negative numbers are for sending back to onion
	public Text[] pikminNumbers;

	void Update ()	{

		//Check if the arrays are matched up
		if(pikminNumbers.Length != pikminToDispense.Length)	{
			print("Mismatching parallel array lengths");
			return;
		}
			
		//Get numbers from text
		for(int i = 0; i<pikminToDispense.Length;i++)	{
			pikminToDispense[i] = int.Parse("0" + pikminNumbers[i].text);
		}

	}


  //This function will be called using the UI system
    public void SignalMasterEnd()
    {
		master.SignalEndGet();
    }

	//This function will be called using the UI system
	public void Dispense ()	{

		print("<color=red>Master must dispense " + pikminToDispense[0] + " Red Pikmin</color>");
		print("<color=yellow>Master must dispense" + pikminToDispense[1] + " Yellow Pikmin</color>");
		print("<color=blue>Master must dispense " + pikminToDispense[2] + " Blue Pikmin</color>");

		SignalMasterEnd();
		master.SignalDispenseGet(pikminToDispense);
	}

   
}
