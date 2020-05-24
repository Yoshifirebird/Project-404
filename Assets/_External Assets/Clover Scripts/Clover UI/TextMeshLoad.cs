/*
 * TextMeshLoad.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Basically just a TextLoader but compatible with TextMesh
 */

using UnityEngine;

public class TextMeshLoad : MonoBehaviour {

	public string path;

	void Start () {

		GetComponent<TextMesh>().text = TextLoader.getText (path, GetComponent<TextMesh>().text);

	}
	

}