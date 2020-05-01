/*
 * Test_PikminAttacking.cs
 * Created by: Ambrosia
 * Created on: 1/5/2020 (dd/mm/yy)
 */

using System.Collections.Generic;
using UnityEngine;

public class Test_PikminAttacking : MonoBehaviour, IPikminAttack {
  public PikminIntention IntentionType => PikminIntention.Attack;

  [Header ("Settings")]
  [SerializeField] float _MaxHealth = 10;
  float _CurrentHealth = 0;

  List<PikminAI> _Attacking = new List<PikminAI> ();

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
  }

  void Update () {
    if (_CurrentHealth <= 0) {

      Destroy (gameObject);
    }
  }
}