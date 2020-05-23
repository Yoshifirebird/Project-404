//This is an object for UI elements to target when instantiating

using UnityEngine;

public class MainGUI : MonoBehaviour {

	public static MainGUI instance;

	void Start () {
		instance = this;
	}

}
