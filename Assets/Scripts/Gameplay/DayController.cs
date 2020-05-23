/*
 * DayController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class DayController : MonoBehaviour
{
  [Header("Debugging")]
  [SerializeField] DayState _State = DayState.Morning;
  [SerializeField] float _CurrentTime = 0;

  void Update()
  {
    _CurrentTime += Time.deltaTime;
  }

  #region Public Functions

  public float GetCurrentTime()
  {
    return _CurrentTime;
  }

  #endregion
}
