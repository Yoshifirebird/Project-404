/*
 * OnionController.cs
 * Created by: Ambrosia / Newgame + LD
 * Created on: 5/9/2020 (dd/mm/yy)
 */

using UnityEngine;

public class OnionController : MonoBehaviour
{
  [Header("Components")]
  [SerializeField] GameObject _MainUI = null;
  [SerializeField] GameObject _MenuUI = null;
  bool _InTrigger = false;

  void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      _InTrigger = true;
    }
  }

  void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      _InTrigger = false;
    }
  }

  void Update()
  {
    if (_InTrigger && Input.GetButtonDown("A Button"))
    {
      GameManager._Player._Paralyzed = !GameManager._Player._Paralyzed;
      _MenuUI.SetActive(GameManager._Player._Paralyzed);
      _MainUI.SetActive(!GameManager._Player._Paralyzed);
    }
  }

  void OnGUI()
  {
    if (GameManager._DebugGui)
    {
      int yOffset = 210;
      GUI.Label(new Rect(400, yOffset, 300, 500), $"OnionDebug Trigger:{_InTrigger} Paralyzed:{GameManager._Player._Paralyzed}");
    }
  }
}
