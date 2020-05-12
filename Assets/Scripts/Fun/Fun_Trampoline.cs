/*
 * Fun_Trampoline.cs
 * Created by: Ambrosia
 * Created on: 12/5/2020 (dd/mm/yy)
 */

using UnityEngine;
using DG.Tweening;

public class Fun_Trampoline : MonoBehaviour
{
  [SerializeField] Transform _EndPosition = null;
  [SerializeField] float _BouncePower = 5;
  [SerializeField] float _BounceTime = 2.5f;

  void OnTriggerEnter(Collider other)
  {
    other.transform.DOJump(_EndPosition.position + Vector3.up / 2, _BouncePower, 1, _BounceTime);
  }
}
