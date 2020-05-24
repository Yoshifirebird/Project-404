/*
 * TextVertexAccess.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * A modified script from the Gradient effects from Sonic '06 PC. This accesses the UI Text verticies individually.
 * Particularly used by the TextBoxUI script
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TextVertexAccess : BaseMeshEffect
{
	public Text textField;
	public int vertIndex;
	public int vertMax;






	public override void ModifyMesh(VertexHelper helper)
	{
		//print ("Modify the mesh!");

		textField.color = Color.white;

		if (!IsActive() || helper.currentVertCount == 0)
			return;

		List<UIVertex> vertices = new List<UIVertex>();
		helper.GetUIVertexStream(vertices);

		//float bottomY = vertices[0].position.y;
		//float topY = vertices[0].position.y;
		//print(helper.currentVertCount);
		vertMax = Mathf.RoundToInt(helper.currentVertCount);
		vertIndex = Mathf.Clamp (vertIndex,0,vertMax);



		UIVertex v = new UIVertex();

		for (int i = 0; i < vertMax; i++)
		{
			//Color32 defaultVColor = v.color;
			//vertices[Mathf.RoundToInt(i*4)].color = 
			//print(i);
			helper.PopulateUIVertex(ref v, i);
			v.color = (i < vertIndex) ? Color.white : Color.clear;
			helper.SetUIVertex(v, i);
		}

	}


}