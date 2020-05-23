/*
 * Fun_Trampoline.cs
 * Created by: Ambrosia
 * Created on: 12/5/2020 (dd/mm/yy)
 */

using DG.Tweening;
using UnityEngine;

public class Fun_Trampoline : MonoBehaviour {
  [SerializeField] float _BouncePower = 5;

  void OnTriggerStay (Collider other) {
    Rigidbody rb = other.GetComponent<Rigidbody> ();
    if (rb != null) {
      rb.velocity = new Vector3 (rb.velocity.x, _BouncePower, rb.velocity.z);
    }
  }
}
