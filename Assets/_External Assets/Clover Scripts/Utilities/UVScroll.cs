/*
 * UVScroll.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Scroll some textures out of shader
 */

using UnityEngine;

public class UVScroll : MonoBehaviour {

  [System.Serializable]
  public class ScrollCategory {

    public Renderer renderer;
    public string texName = ("_MainTex");
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.5f;
    public int materialIndex;

  }
  public ScrollCategory[] scrollRenderer;

  void Start () {

    for (int i = 0; i < scrollRenderer.Length; i++) {
      if (scrollRenderer[i].renderer == null)
        scrollRenderer[i].renderer = GetComponent<Renderer> ();

    }

  }

  void Update () {

    for (int i = 0; i < scrollRenderer.Length; i++) {

      float offsetu = Time.time * scrollRenderer[i].scrollSpeedX % 2;
      float offsetv = Time.time * scrollRenderer[i].scrollSpeedY % 2;

      scrollRenderer[i].renderer.materials[scrollRenderer[i].materialIndex].SetTextureOffset (scrollRenderer[i].texName,
        new Vector2 (offsetu, offsetv));

    }

  }
}
