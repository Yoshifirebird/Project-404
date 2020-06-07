/*
 * Fun_Manager.cs
 * Created by: Ambrosia
 * Created on: 13/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class Fun_Manager : MonoBehaviour {
  [Header ("Settings")]
  [SerializeField] KeyCode _KeyToEnable = KeyCode.Alpha2;
  [SerializeField] GameObject[] _FunObjects = null;

  void Awake () {
    foreach (var obj in _FunObjects) {
      obj.SetActive (GameManager._FunMode);
    }
  }

  void Update () {
    if (Input.GetKeyDown (_KeyToEnable)) {
      GameManager._FunMode = !GameManager._FunMode;

      foreach (var obj in _FunObjects) {
        obj.SetActive (GameManager._FunMode);
      }
    }
  }
}
