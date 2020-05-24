/*
 * DayController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class DayController : MonoBehaviour {
  [Header ("Debugging")]
  [SerializeField] DayState _State = DayState.Morning;
  [SerializeField] float _currentTime = 0;

  public float _CurrentTime => _currentTime;

  void Awake() {
    GameManager._DayController = this;
  }

  void Update () {
    _currentTime += Time.deltaTime;
  }
}
