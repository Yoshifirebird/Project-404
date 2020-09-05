/*
 * OnionController.cs
 * Created by: Ambrosia
 * Created on: 5/9/2020 (dd/mm/yy)
 */

using UnityEngine;

public class OnionController : MonoBehaviour
{
  [Header("Components")]
  [SerializeField] GameObject _UIComponentsParent = null;

  bool _InTrigger = false;
  PlayerMovementController _Movement = null;

  void Start()
  {
    _Movement = GameManager._Player.GetComponent<PlayerMovementController>();
  }

  void Update()
  {
    if (_InTrigger)
    {
      if (Input.GetButtonDown("A Button"))
      {
        GameManager._Player._Paralyzed = !GameManager._Player._Paralyzed;
        _UIComponentsParent.SetActive(GameManager._Player._Paralyzed);
      }
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (!other.CompareTag("Player"))
    {
      return;
    }

    _InTrigger = true;
  }

  void OnTriggerExit(Collider other)
  {
    if (!other.CompareTag("Player"))
    {
      return;
    }

    _InTrigger = false;
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
