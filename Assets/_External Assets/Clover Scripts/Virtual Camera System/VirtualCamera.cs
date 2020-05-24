/*
 * VirtualCamera.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * I bet some peeps will see this as a lite Cinemachine
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCamera : MonoBehaviour {

  public float fieldOfView = 60;
  public float setDuration;
  public AnimationCurve setCurve;
  public bool active;

  public Coroutine camEvent;

  //public void setCamera (ActorBase actorTarget)	{

  //print ("Setting camera!");
  //setCameraTask (actorTarget.cam);

  //}

  public void setCameraDirect (VirtualCameraTarget cam) {

    print ("Setting camera directly!");
    setCameraTask (cam);

  }

  public void setCameraMain () {

    print ("Setting main camera!");
    setCameraTask (MainCamera.instance.vrCam);

  }

  public void setCameraTask (VirtualCameraTarget cam) {
    if (camEvent != null) StopCoroutine (camEvent);
    cam.changeCameras (this);
    camEvent = StartCoroutine (lerpCamera (cam, setCurve, setDuration));

  }

  IEnumerator lerpCamera (VirtualCameraTarget target, AnimationCurve curve, float setTime) {

    float timeElapsed = 0;

    while (timeElapsed < setDuration) {

      timeElapsed += Time.deltaTime;
      target.interpolation = curve.Evaluate (timeElapsed / setTime);

      yield return new WaitForEndOfFrame ();

    }

    target.interpolation = 1;

  }

  //public void resetCamera (ActorBase actorTarget)	{

  //print ("Reset camera!");
  //resetCameraTask (actorTarget.cam);

  //}

  public void resetCameraDirect (VirtualCameraTarget cam) {

    print ("Reset camera directly!");
    resetCameraTask (cam);

  }

  public void resetCameraMain () {

    print ("Reset main camera!");
    resetCameraTask (MainCamera.instance.vrCam);

  }

  public void resetCameraTask (VirtualCameraTarget cam) {

    if (camEvent != null) StopCoroutine (camEvent);
    cam.changeCameras (cam.defaultCam);
    camEvent = StartCoroutine (lerpCamera (cam,
      cam.defaultCam.setCurve, cam.defaultCam.setDuration));

  }

}
