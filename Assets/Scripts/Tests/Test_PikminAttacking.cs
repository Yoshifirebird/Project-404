/*
 * Test_PikminAttacking.cs
 * Created by: Ambrosia
 * Created on: 1/5/2020 (dd/mm/yy)
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test_PikminAttacking : MonoBehaviour, IPikminAttack {
  public PikminIntention IntentionType => PikminIntention.Attack;

  [Header ("Components")]
  [SerializeField] TextMeshPro _Text = null;

  [Header ("Settings")]
  [SerializeField] float _MaxHealth = 10;
  [SerializeField] float _MoveSpeed = 5;

  [Header ("Debugging")]
  [SerializeField] float _CurrentHealth = 0;
  [SerializeField] Vector3 _NextPosition = Vector3.down * 100;

  List<PikminAI> _Attacking = new List<PikminAI> ();
  Rigidbody _Rigidbody = null;

  public void OnAttackEnd (PikminAI pikmin) {
    _Attacking.Remove (pikmin);
  }

  public void OnAttackRecieve (float damage) {
    _CurrentHealth -= damage;
  }

  public void OnAttackStart (PikminAI pikmin) {
    _Attacking.Add (pikmin);
  }

  void Awake () {
    _CurrentHealth = _MaxHealth;
    _Rigidbody = GetComponent<Rigidbody>();
  }

  void Update () {
    _Text.text = "Health: " + _CurrentHealth + '\n' + "Attached: " + _Attacking.Count;
    
    if (MathUtil.DistanceTo(transform.position, _NextPosition) < 1)
    {
      // Pick a new destination
      Vector3 newPosition = _NextPosition - Vector3.down * 100;
      while (!Physics.Raycast(newPosition, Vector3.down))
      {
        newPosition = MathUtil.XZToXYZ(Random.insideUnitCircle * 2.5f, 2.5f);
      }
      _NextPosition = newPosition;
    }
    
    if (_CurrentHealth <= 0) {

      foreach (var pikmin in _Attacking) {
        pikmin.LatchOnto (null);
      }

      Destroy (gameObject);
    }

    Vector3 delta = _NextPosition - transform.position;
    delta.y = _Rigidbody.velocity.y;
    _Rigidbody.velocity = delta;

    delta.y = 0;
    transform.rotation = Quaternion.LookRotation(delta);
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.DrawCube(_NextPosition, Vector3.one * 5);

    // Pick a new destination
    Vector3 newPosition = _NextPosition - Vector3.down * 100;
    while (!Physics.Raycast(_NextPosition, Vector3.down))
    {
      _NextPosition = MathUtil.XZToXYZ(Random.insideUnitCircle * 10f, 2.5f);
      Gizmos.DrawCube(newPosition, Vector3.one * 5);
    }
  }
}
