/*
 * FormationCenter.cs
 * Created by: Kman, Ambrosia
 * Created on: 26/7/2020 (dd/mm/yy)
 */

using UnityEngine;

public class FormationCenter : MonoBehaviour {
  [SerializeField] float _IterationScale = 1;

  //Relative points to avoid calculating them each frame

  public Vector3 GetPositionAt (int index) {
    int currentOnLevel = index;
    int maxOnLevel = 4;
    int currentIteration = 1;

    while (currentOnLevel >= maxOnLevel) {
      currentOnLevel -= maxOnLevel;
      maxOnLevel += 4;
      currentIteration++;
    }

    return transform.position + MathUtil.XZToXYZ (MathUtil.PositionInUnit (maxOnLevel, currentOnLevel) * currentIteration * _IterationScale);
  }
}
