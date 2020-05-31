/*
 * Player.cs
 * Created by: Ambrosia
 * Created on: 20/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public class Player : MonoBehaviour, IHealth {
  [Header ("Components")]
  public Transform _FormationCentre = null;
  [SerializeField] AudioClip _LowHealthClip = null;
  AudioSource _AudioSource = null;

  [Header ("Settings")]
  [SerializeField] float _MaxHealth = 100;
  [SerializeField] float _LowHealthDelay = 1;

  [Header ("Debugging")]
  [SerializeField] float _CurrentHealth = 0;

  float _LowHealthAudioTimer = 0;

  void Awake () {
    _CurrentHealth = _MaxHealth;

    _AudioSource = GetComponent<AudioSource> ();

    // Apply singleton pattern to allow for the objects in the scene to reference the active Player instance
    if (GameManager._Player == null) {
      GameManager._Player = this;
    }
    else {
      Destroy (gameObject);
    }
  }

  void Update () {
    // Tab pauses the game, experimental
    if (Application.isEditor) {
      if (Input.GetKeyDown (KeyCode.Tab)) {
        GameManager._IsPaused = !GameManager._IsPaused;
        Time.timeScale = GameManager._IsPaused ? 0 : 1;
      }

      if (Input.GetKeyDown (KeyCode.Alpha3)) {
        SubtractHealth (_MaxHealth / 10);
      }
    }

    if (_CurrentHealth <= 0) {
      Debug.Log ("Player is dead!");
      Debug.Break ();
    }

    // If the health is less that a quarter of what the max is
    if (_CurrentHealth / _MaxHealth < 0.25f) {
      _LowHealthAudioTimer += Time.deltaTime;
      if (_LowHealthAudioTimer >= _LowHealthDelay) {
        _LowHealthAudioTimer = 0;
        _AudioSource.PlayOneShot (_LowHealthClip);
      }
    }
  }

  #region Health Implementation

  public float GetCurrentHealth () {
    return _CurrentHealth;
  }

  public float GetMaxHealth () {
    return _MaxHealth;
  }

  public void SetHealth (float h) {
    _CurrentHealth = h;
  }

  public float AddHealth (float h) {
    _CurrentHealth += h;
    return _CurrentHealth;
  }

  public float SubtractHealth (float h) {
    _CurrentHealth -= h;
    return h;
  }

  #endregion
}
