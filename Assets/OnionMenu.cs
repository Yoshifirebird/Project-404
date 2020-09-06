/*
 * OnionMenu.cs
 * Created by: Newgame+ LD
 * Created on: 9/5/2020 (dd/mm/yy)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionMenu : MonoBehaviour
{

	public OnionController master;

    // Start is called before the first frame update
    public void SignalMasterEnd()
    {
		master.signalEndGet();
    }

   
}
