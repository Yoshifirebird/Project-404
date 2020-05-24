/*
 * This script is a leftover from Pikmin Colony and should be ignored
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapObject : MonoBehaviour {

  public Transform reference;
  public RectTransform uiElement;
  public Camera mainCamera;
  public Vector3 offset;

  // Use this for initialization
  void Start () {

    mainCamera = Camera.main;

  }

  // Update is called once per frame
  void LateUpdate () {

    if (reference) {
      Vector3 input = mainCamera.WorldToViewportPoint (reference.position + offset);

      //input = new Vector2 (input.x / Screen.width, input.y / Screen.height);

      uiElement.anchorMax = (Vector2) input;
      uiElement.anchorMin = (Vector2) input;

      uiElement.gameObject.SetActive (input.z >= 0);
    }
  }

}
