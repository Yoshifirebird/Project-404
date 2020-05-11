/*
 * Player.cs
 * Created by: Ambrosia
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using UnityEngine;

public class Player : MonoBehaviour {
  void Awake () {
    // Apply singleton pattern to allow for the objects in the scene to reference the active Player instance
    if (GameManager._Player == null) {
      GameManager._Player = this;
    } else {
      Destroy (gameObject);
    }
  }

  void Update()
  {
    // Tab pauses the game, experimental
    if (Application.isEditor && Input.GetKeyDown(KeyCode.Tab))
    {
      GameManager._IsPaused = !GameManager._IsPaused;
      Time.timeScale = GameManager._IsPaused ? 0 : 1;
    }
  }
}