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
  [SerializeField] float _MaxWhistleDistance = 5;
  [SerializeField] LayerMask _WhistleInteractLayer = 0;
  [SerializeField] LayerMask _WhistleBlowInteractLayer = 0;

  [Header ("Debug")]
  [SerializeField] bool _Blowing = false;
  [SerializeField] float _CurrentTime = 0;

  float _TotalBlowTime = 0;

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

      _Blowing = true;
    }
    if (Input.GetButtonUp ("B Button") || _CurrentTime >= _TotalBlowTime) {
      // Stop the particles
      foreach (var ps in _WhistleParticles) {
        if (ps.isPlaying) {
          ps.Stop ();
        }
      }

      _CurrentTime = 0;
      _Blowing = false;
    }

    // Move the Player 
    Ray ray = _MainCamera.ScreenPointToRay (Input.mousePosition);
    if (Physics.Raycast (ray, out RaycastHit hit, _MaxWhistleDistance, _WhistleInteractLayer, QueryTriggerInteraction.Ignore)) {
      transform.position = hit.point;
    }

    if (_Blowing) {
      _CurrentTime += Time.deltaTime;

      float frac = (_CurrentTime / _TotalBlowTime);

      _ParentParticle.localScale = MathUtil.XZToXYZ (Vector2.one * frac, frac);
    }
  }
}
