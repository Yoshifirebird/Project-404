/*
 * Test_OrbitFormation.cs
 * Created by: Ambrosia
 * Created on: 26/7/2020 (dd/mm/yy)
 */

using UnityEngine;

public class Test_OrbitFormation : MonoBehaviour {
  [SerializeField] int _AmountInSquad = 50;

  Vector3 GetPositionAt (int index) {
    int currentOnLevel = index;
    int maxOnLevel = 4;
    int currentIteration = 1;

    while (currentOnLevel >= maxOnLevel) {
      currentOnLevel -= maxOnLevel;
      maxOnLevel += 4;
      currentIteration++;
    }

    return MathUtil.XZToXYZ (MathUtil.PositionInUnit (maxOnLevel, currentOnLevel) * currentIteration);
  }

  void OnDrawGizmosSelected () {
    for (int i = 0; i < _AmountInSquad; i++) {
      Gizmos.DrawWireSphere (transform.position + GetPositionAt (i), 1);
    }
  }
}
