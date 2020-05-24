using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageScroll : MonoBehaviour {

  [System.Serializable]
  public class ScrollCategory {

    public bool unscaled = true;
    public RawImage rawImage;
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.5f;

  }
  public ScrollCategory[] scrollRenderer;

  void Update () {

    for (int i = 0; i < scrollRenderer.Length; i++) {

      float offsetu = ((scrollRenderer[i].unscaled) ? Time.unscaledTime : Time.time) * scrollRenderer[i].scrollSpeedX % 1;
      float offsetv = ((scrollRenderer[i].unscaled) ? Time.unscaledTime : Time.time) * scrollRenderer[i].scrollSpeedY % 1;

      scrollRenderer[i].rawImage.uvRect = new Rect (scrollRenderer[i].scrollSpeedX != 0 ? offsetu : scrollRenderer[i].rawImage.uvRect.x,
        scrollRenderer[i].scrollSpeedY != 0 ? offsetv : scrollRenderer[i].rawImage.uvRect.y,
        scrollRenderer[i].rawImage.uvRect.width,
        scrollRenderer[i].rawImage.uvRect.height);

    }

  }
}
