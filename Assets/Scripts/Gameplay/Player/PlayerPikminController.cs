/*
 * PlayerPikminController.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class PlayerPikminController : MonoBehaviour {
  [Header ("Throwing")]
  [SerializeField] float _PikminGrabRadius = 5;
  [SerializeField] float _MaxGrabHeight = 1;
  [SerializeField] Transform _ReticleTransform = null;

  [Header("Punching")]
  [SerializeField] float _PunchDamage = 1;
  [SerializeField] float _PunchCooldown = 0.5f;
  [SerializeField] Vector3 _PunchBoxSize = Vector3.one / 2;
  [SerializeField] float _PunchBoxFwdOffset = 1; // Relative to transform.forward
  [SerializeField] LayerMask _PunchAffectedLayers = 0;

  [Header ("Debug")]
  [SerializeField] int _GrabFXQuality = 50;
  [SerializeField] GameObject _HoldingPikmin = null;
  [SerializeField] float _PunchTimer = 0;
  PikminAI _HoldingPikminAI = null;

  void Awake() {
    Debug.Assert(_ReticleTransform != null);
  }

  void Update () {
    if (Input.GetButtonDown ("X Button")) {
      PikminStatsManager.ClearSquad ();
    }

    bool attemptedGrab = false;

    if (Input.GetButtonDown ("A Button")) {
      attemptedGrab = true;
      if (PikminStatsManager.GetTotalInSquad () > 0) {
        _HoldingPikmin = GetClosestPikmin ();
        if (_HoldingPikmin != null) {
          _HoldingPikminAI = _HoldingPikmin.GetComponent<PikminAI> ();
          _HoldingPikminAI.StartThrowHold ();
        }
      }
    }

    if (_HoldingPikmin != null) {
      // TODO: Add throwing
      if (Input.GetButton ("A Button")) {
        _HoldingPikmin.transform.position = transform.position + transform.forward;
      }
      else {
        // Throw the Pikmin!
        _HoldingPikminAI.EndThrowHold (_ReticleTransform.position);

        _HoldingPikmin = null;
        _HoldingPikminAI = null;
      }
    }
    else if (attemptedGrab && _PunchTimer <= 0) {
      // We couldn't grab a Pikmin, we can punch now

      Collider[] pCollided = Physics.OverlapBox(transform.position + transform.forward - transform.right / 2, _PunchBoxSize / 2, transform.rotation, _PunchAffectedLayers);
      foreach (var pHit in pCollided)
      {
        var hComponent = pHit.GetComponent<IHealth>();
        
        // Hit the object!
        hComponent.SubtractHealth(_PunchDamage);
        // TODO: figure out a particular system to allow for custom hit particles / sounds
      }

      _PunchTimer = _PunchCooldown;
    }

    // Decrease the punch timer if we've punched, effectively acts as a cooldown so you can't spam
    if (_PunchTimer > 0)
    {
      _PunchTimer -= Time.deltaTime;
    }
  }

  void OnGUI () {
    if (GameManager._DebugGui) {
      int yOffset = 210;
      GUI.Label (new Rect (10, yOffset, 300, 500), PikminStatsManager._RedStats.ToString ());
      GUI.Label (new Rect (10, yOffset + 70, 300, 500), PikminStatsManager._YellowStats.ToString ());
      GUI.Label (new Rect (10, yOffset + 140, 300, 500), PikminStatsManager._BlueStats.ToString ());
    }
  }

  void OnDrawGizmosSelected () {
    Gizmos.color = Color.green;
    for (int i = 0; i < _GrabFXQuality; i++) {
      Vector3 endPosition = transform.position + MathUtil.XZToXYZ (_PikminGrabRadius * MathUtil.PositionInUnit (_GrabFXQuality, i));

      // Visualise the upper bounds of the grab height limitation
      Gizmos.DrawLine (endPosition, endPosition + (Vector3.up * _MaxGrabHeight));
      Gizmos.DrawLine (endPosition + (Vector3.up * _MaxGrabHeight), transform.position);

      // Visualise the lower bounds of the grab height limitation
      Gizmos.DrawLine (endPosition, endPosition + (Vector3.down * _MaxGrabHeight));
      Gizmos.DrawLine (endPosition + (Vector3.down * _MaxGrabHeight), transform.position);
    }

    Gizmos.color = Color.red;
    Gizmos.DrawCube(transform.position + transform.forward - transform.right / 2, _PunchBoxSize);
  }

  GameObject GetClosestPikmin () {
    // C = closest, Pik = Pikmin, Dist = Distance
    GameObject cPik = null;
    float cPikDist = Mathf.Pow(_PikminGrabRadius, 2);

    foreach (var pikmin in PikminStatsManager._InSquad) {
      // Check the height difference between the Pikmin and the Player
      float yDist = Mathf.Abs (transform.position.y - pikmin.transform.position.y);

      if (yDist >= _MaxGrabHeight) {
        continue;
      }

      // Check the distance between the Pikmin and the Player
      float pikDist = MathUtil.DistanceTo (transform.position, pikmin.transform.position, false);

      if (pikDist < cPikDist) {
        cPikDist = pikDist;
        cPik = pikmin;
      }
    }

    return cPik;
  }
}
