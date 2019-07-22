using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningCube : MonoBehaviour {
	//Simply attach this to a gameObject and you're good to go!
    //You can adjust the rotation values in the Unity Editor
    [SerializeField] private float xAxisSpeed = 50f; //X Axis Rotation Speed
    [SerializeField] private float yAxisSpeed = 50f; //Y Axis Rotation Speed
    [SerializeField] private float zAxisSpeed = 50f; //Z Axis Rotation Speed

    void Update () {
        transform.Rotate(Vector3.up, xAxisSpeed * Time.deltaTime); //Rotate from the Y axis
        transform.Rotate(Vector3.left, yAxisSpeed * Time.deltaTime); //Rotate from the X axis
        transform.Rotate(Vector3.forward, zAxisSpeed * Time.deltaTime); //Rotate from the Z axis
    }
}