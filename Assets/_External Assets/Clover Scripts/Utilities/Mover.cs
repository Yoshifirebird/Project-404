/*
 * Mover.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 * 
 * Move an object every frame
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public Vector3 rotSpeed;
	public bool local;

	// Update is called once per frame
	void Update () {

		if(local)transform.Rotate (Time.deltaTime * rotSpeed, Space.Self);
		else transform.Rotate (Time.deltaTime * rotSpeed, Space.World);
	}
}
