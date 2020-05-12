/*
 * WhistleController.cs
 * Created by: Ambrosia
 * Created on: 12/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class WhistleController : MonoBehaviour {
  [Header ("Components")]
  [SerializeField] Transform _ParentParticle = null;
  [SerializeField] ParticleSystem[] _WhistleParticles = null;

  [Header ("Settings")]
  [SerializeField] float _StartRadius = 0.5f;
  [SerializeField] float _MaxRadius = 3;
  // Thanks The JustGreat Minty Meeo for Pikmin 1 values on the whistle
  [SerializeField] float _ExpandBlowTime = 0.45f;
  [SerializeField] float _HoldingBlowTime = 1;
  [SerializeField] float _MaxWhistleDistance = Mathf.Infinity;
  [SerializeField] LayerMask _WhistleInteractLayer = 0;
  [SerializeField] LayerMask _WhistleBlowInteractLayer = 0;

  [Header ("Debug")]
  [SerializeField] bool _Blowing = false;
  [SerializeField] float _CurrentTime = 0;
  [SerializeField] float _CurrentRadius = 0;
  [SerializeField] float _TotalBlowTime = 0;

  Camera _MainCamera = null;

  void Awake () {
    _MainCamera = Camera.main;
    _TotalBlowTime = _ExpandBlowTime + _HoldingBlowTime;

    foreach (var ps in _WhistleParticles) {
      ParticleSystem.MainModule newMain = ps.main;
      newMain.duration = _TotalBlowTime;
    }
  }

  void Update () {
    if (Input.GetButtonDown ("B Button")) {
      // Start the particles
      foreach (var ps in _WhistleParticles) {
        if (ps.isPlaying) {
          ps.Stop ();
        }

        ps.Play ();
      }
      
      print("Starting particle system");
      _CurrentRadius = _StartRadius;
      _ParentParticle.localScale = MathUtil.XZToXYZ(Vector2.one * _StartRadius, _StartRadius);
      _Blowing = true;
    }
    if (Input.GetButtonUp ("B Button") || _CurrentTime >= _TotalBlowTime) {
      // Stop the particles
      foreach (var ps in _WhistleParticles) {
        if (ps.isPlaying) {
          ps.Stop ();
        }
      }

      print("Stopping particle system");
      _CurrentTime = 0;
      _Blowing = false;
    }

    // Move the Player 
    Ray ray = _MainCamera.ScreenPointToRay (Input.mousePosition);
    if (Physics.Raycast (ray, out RaycastHit hit, _MaxWhistleDistance, _WhistleInteractLayer, QueryTriggerInteraction.Ignore)) {
      transform.position = hit.point + Vector3.up * 2;
    }

    if (_Blowing) {
      _CurrentTime += Time.deltaTime;
      float time = (_StartRadius + (_CurrentTime / _TotalBlowTime)) * _MaxRadius;

      _CurrentRadius = time;

      _ParentParticle.localScale = MathUtil.XZToXYZ (Vector2.one * _CurrentRadius, _CurrentRadius);
    }
  }
}
