using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_Move_Left : PhysicsObject {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        targetVelocity = Vector2.right;
	}
}
