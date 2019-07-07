using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningCube : MonoBehaviour {
	//Simply attach this to a gameObject and you're good to go!
    public float speed = 50f; //Rotation Speed

	void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime); //Rotate from the Y axis
        transform.Rotate(Vector3.left, speed * Time.deltaTime); //Rotate from the X axis
        transform.Rotate(Vector3.forward, speed * Time.deltaTime); //Rotate from the Z axis
    }
}