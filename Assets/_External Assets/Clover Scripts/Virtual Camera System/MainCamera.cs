/*
 * MainCamera.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Always put this on the main scene camera
 */

using UnityEngine;

public class MainCamera : MonoBehaviour {
  public static MainCamera instance;
  public Camera cam;
  public VirtualCameraTarget vrCam;

  void Start () {
    if (MainCamera.instance == null)
      MainCamera.instance = this;
    else Destroy (gameObject);
  }
}
