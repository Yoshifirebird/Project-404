/*
 * Player.cs
 * Created by: Ambrosia
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using UnityEngine;

public class Player : MonoBehaviour, IHealth {
  [Header ("Components")]
  public Transform _FormationCentre = null;

  [Header ("Settings")]
  [SerializeField] float _MaxHealth = 100;

  [Header ("Debugging")]
  [SerializeField] float _CurrentHealth = 0;

  void Awake () {
    _CurrentHealth = _MaxHealth;

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
    if (Application.isEditor && Input.GetKeyDown (KeyCode.Tab)) {
      GameManager._IsPaused = !GameManager._IsPaused;
      Time.timeScale = GameManager._IsPaused ? 0 : 1;
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
