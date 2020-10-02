/*
 * OnionController.cs
 * Created by: Ambrosia / Newgame + LD
 * Created on: 5/9/2020 (dd/mm/yy)
 */

using UnityEngine;

public class OnionController : MonoBehaviour
{
  [Header("Components")]
  [SerializeField] GameObject _UIComponentsParent = null;

  //bool _InTrigger = false;
  PlayerMovementController _Movement = null;
	public OnionMenu menuSpawn;

  void Start()
  {
    _Movement = GameManager._Player.GetComponent<PlayerMovementController>();
  }
		

	public void CallMenu ()	{

		GameManager._Player._Paralyzed = true;
		OnionMenu newMenu = Instantiate(menuSpawn, MainGUI.instance.transform).GetComponent<OnionMenu>();

		newMenu.master = this;
	}


	public void SignalEndGet ()	{

		GameManager._Player._Paralyzed = false;

	}


	/*
  void OnGUI()
  {
    if (GameManager._DebugGui)
    {
      int yOffset = 210;
      GUI.Label(new Rect(400, yOffset, 300, 500), $"OnionDebug Trigger:{_InTrigger} Paralyzed:{GameManager._Player._Paralyzed}");
    }
  }
  */
}
