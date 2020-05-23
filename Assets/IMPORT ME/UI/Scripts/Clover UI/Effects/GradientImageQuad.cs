 using UnityEngine;
 using System.Collections.Generic;
 using UnityEngine.UI;
 
 [AddComponentMenu("UI/Effects/Image Gradient Quad")]
 public class GradientImageQuad : BaseMeshEffect
 {
	public List<Color> grad;


	public override void ModifyMesh(VertexHelper helper)
	{
		if (!IsActive() || helper.currentVertCount == 0)
			return;

		List<UIVertex> vertices = new List<UIVertex>();
		helper.GetUIVertexStream(vertices);

		UIVertex v = new UIVertex();

		for (int i = 0; i < helper.currentVertCount; i++)
		{
			helper.PopulateUIVertex(ref v, i);
			v.color = grad [i];
			helper.SetUIVertex(v, i);
		}
	}
 }