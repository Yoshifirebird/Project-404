using UnityEngine;

public class DestroySameTag : MonoBehaviour {

	public bool destroyThis;	//Do we destroy this object?

	// Use this for initialization
	void Start () {

		GameObject target = GameObject.FindWithTag (gameObject.tag);

		if (target != null && target != this.gameObject) {		//If there's already an object with the same tag...

			if (!destroyThis) {		//Should we destroy that one?

				Destroy (target);

			} else {	//Or this one?

				Destroy (gameObject);

			}

		}

	}
	

}
