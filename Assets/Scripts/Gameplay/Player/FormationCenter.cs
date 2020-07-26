 /*
 * FormationCenter.cs
 * Created by: Kman
 * Created on: 26/7/2020 (dd/mm/yy)
 */


using UnityEngine;

public class FormationCenter : MonoBehaviour
{
  //Affects how close the points can be to one another
  [SerializeField] float _PointRadius;
  [SerializeField] GameObject _TargetObject;
  //Relative points to avoid calculating them each frame
  public Transform[] _Positions;

  void Awake()
  {
    _Positions = new Transform[100];
    AssignPositions();
  }

  void AssignPositions()
  {
    for(int i = 0; i < _Positions.Length; i++)
    {
      _Positions[i] = Instantiate(_TargetObject, transform).transform;
      _Positions[i].localPosition = Vector3.forward * i * _PointRadius;
    }

  }
}
