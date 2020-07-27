/*
 * Test_OrbitFormation.cs
 * Created by: Ambrosia
 * Created on: 26/7/2020 (dd/mm/yy)
 */

using UnityEngine;

public class Test_OrbitFormation : MonoBehaviour {
  [SerializeField] int _AmountInSquad = 50;

  void OnDrawGizmosSelected () {
    int currentOnLevel = 0;
    int maxOnLevel = 4;
    int currentIteration = 1;
    for (int i = 0; i < _AmountInSquad; i++) {
      if (currentOnLevel >= maxOnLevel) {
        maxOnLevel += 4;
        currentIteration++;
        currentOnLevel = 0;
      }
      currentOnLevel++;

      Vector3 offset = MathUtil.XZToXYZ (MathUtil.PositionInUnit (maxOnLevel, currentOnLevel) * currentIteration);
      Gizmos.DrawWireSphere (transform.position + offset, 1);
    }
  }
}
