/*
 * DontDestroyOnLoad.cs
 * Created by: ???
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * This should be self explanitory
 */

using UnityEngine;
public class DontDestroyOnLoad : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}