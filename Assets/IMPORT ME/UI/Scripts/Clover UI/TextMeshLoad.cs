using UnityEngine;

public class TextMeshLoad : MonoBehaviour {

	public string path;

	void Start () {

		GetComponent<TextMesh>().text = TextLoader.getText (path, GetComponent<TextMesh>().text);

	}
	

}