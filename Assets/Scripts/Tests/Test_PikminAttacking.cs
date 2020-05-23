/*
 * Test_PikminAttacking.cs
 * Created by: Ambrosia
 * Created on: 1/5/2020 (dd/mm/yy)
 */

using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

// A roaming object that will move from place to place randomly
public class Test_PikminAttacking : MonoBehaviour, IPikminAttack {
  public PikminIntention IntentionType => PikminIntention.Attack;

  [Header ("Components")]
  [SerializeField] TextMeshPro _Text = null;

  [Header ("Movement")]
  [SerializeField] float _MaxMovementSpeed = 5;
  [SerializeField] float _AccelerationSpeed = 1;
  [SerializeField] float _RotationSpeed = 2.5f;

  [Header ("Settings")]
  [SerializeField] float _MaxHealth = 10;
  [SerializeField] float _RandomPositionModifier = 10;

  [Header ("Debugging")]
  [SerializeField] bool _RecalculatePositionDebug = false;
  [SerializeField] float _CurrentHealth = 0;
  [SerializeField] Vector3 _NextPosition = Vector3.down * 100;

  float _CurrentMoveSpeed = 0;

  List<PikminAI> _Attacking = new List<PikminAI> ();
  Rigidbody _Rigidbody = null;

  public void OnAttackEnd (PikminAI pikmin) {
    _Attacking.Remove (pikmin);
  }

  public void OnAttackRecieve (float damage) {
    print ("Recieving attack");
    _CurrentHealth -= damage;
  }

  public void OnAttackStart (PikminAI pikmin) {
    _Attacking.Add (pikmin);
  }

  void Awake () {
    _CurrentHealth = _MaxHealth;
    _Rigidbody = GetComponent<Rigidbody> ();
  }

  void Update () {
    _Text.text = "Health: " + _CurrentHealth + '\n' + "Attached: " + _Attacking.Count;

    if (_CurrentHealth <= 0) {

      foreach (var pikmin in _Attacking) {
        pikmin.LatchOnto (null);
      }

      Destroy (gameObject);
    }

    if (MathUtil.DistanceTo (transform.position, _NextPosition, false) <= 0.25f) {
      RecalculatePosition ();
    }
  }

  void FixedUpdate () {
    // Rotate to look at the object we're moving towards
    Vector3 delta = (_NextPosition - transform.position).normalized;
    delta.y = 0;
    transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (delta), _RotationSpeed * Time.deltaTime);

    // To prevent instant, janky movement we step towards the resultant max speed according to _Acceleration
    _CurrentMoveSpeed = Mathf.SmoothStep (_CurrentMoveSpeed, _MaxMovementSpeed, _AccelerationSpeed * Time.deltaTime);

    Vector3 newVelocity = delta.normalized * _CurrentMoveSpeed;
    newVelocity.y = _Rigidbody.velocity.y;
    _Rigidbody.velocity = newVelocity;
  }

  void OnDrawGizmosSelected () {
    Gizmos.DrawSphere (_NextPosition, transform.localScale.x);
    Gizmos.DrawLine (transform.position, _NextPosition);

    if (_RecalculatePositionDebug) {
      _RecalculatePositionDebug = false;
      RecalculatePosition ();
    }
  }

  public void RecalculatePosition () {
    print ("Recalculating Position");

    Vector3 nextPosition = Vector3.zero;
    // Keep running until we find an acceptable position
    while (true) {
      nextPosition = transform.position + (Random.insideUnitSphere * _RandomPositionModifier);

      // Can we even see the spot?
      if (Physics.Raycast (transform.position, (nextPosition - transform.position).normalized, out RaycastHit hitInfo)) {
        continue;
      }

      // Can we go there without falling into the abyss?
      if (!Physics.Raycast (nextPosition, Vector3.down, out RaycastHit info, transform.localScale.y)) {
        continue;
      }

      // Is the spot we've picked on ourself?
      if (info.transform == transform) {
        continue;
      }

      // We're good to go :)
      break;
    }

    _NextPosition = nextPosition;
  }
}
