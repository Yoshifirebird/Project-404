 /*
  * FormationCenter.cs
  * Created by: Kman
  * Created on: 26/7/2020 (dd/mm/yy)
  */

 using UnityEngine;

 public class FormationCenter : MonoBehaviour {
  [SerializeField] GameObject _TargetObject = null;
  [SerializeField] float _IterationScale = 1;

   //Relative points to avoid calculating them each frame
   public Transform[] _Positions;

   void Awake () {
     _Positions = new Transform[100];
     AssignPositions ();
   }

   void AssignPositions () {
    //Based from Test_OrbitFormation.cs
    int currentOnLevel = 0;
    int maxOnLevel = 4;
    int currentIteration = 1;
    for (int i = 0; i < _Positions.Length; i++)
    {
      _Positions[i] = Instantiate(_TargetObject, transform).transform;
      if (currentOnLevel >= maxOnLevel)
      {
        maxOnLevel += 4;
        currentIteration++;
        currentOnLevel = 0;
      }
      currentOnLevel++;

      Vector3 offset = MathUtil.XZToXYZ(MathUtil.PositionInUnit(maxOnLevel, currentOnLevel) * currentIteration * _IterationScale);
      _Positions[i].localPosition = offset;
    }
  }
}
