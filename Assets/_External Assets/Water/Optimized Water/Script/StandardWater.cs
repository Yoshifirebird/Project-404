using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardWater : MonoBehaviour {

    public Vector2 currentSpeed;
    public Vector2 altCurrentSpeed;

    private float offsetX;
    private float offsetY;

    private float altOffsetX;
    private float altOffsetY;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update () {
        offsetX += Time.deltaTime * currentSpeed.x;
        offsetY += Time.deltaTime * currentSpeed.y;

        altOffsetX += Time.deltaTime * altCurrentSpeed.x;
        altOffsetY += Time.deltaTime * altCurrentSpeed.y;
        renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
        renderer.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(altOffsetX, altOffsetY));
	}
}
