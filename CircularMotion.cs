using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script make an object go around in a circular motion
//Adjust the width, height and speed variables for interesting results
public class CircularMotion : MonoBehaviour {

    private float timeCounter = 0, i = 0;
    public float speed = 5f;
    public float width = 4f;
    public float height = 7f;

	void Update () {
        timeCounter += Time.deltaTime * speed;

        float x = Mathf.Cos(timeCounter) * width;
        float y = Mathf.Sin(timeCounter) * height;
        float z = -6.92f;

        if (timeCounter >= i) {
            i += 1;
            Debug.Log("Time: " + timeCounter);
        }

        transform.position = new Vector3(x, y, z);
	}
}
