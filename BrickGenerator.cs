using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script generates gameobjects as a wall
//Attach this script to an empty gameobject on your scene
//Adjust the height and weight varibles for interesting results
public class BrickGenerator : MonoBehaviour {
    [SerializeField] GameObject prefab; //The gameobject you want to you on the wall (preferably a 3D box shaped like a brick)
    [SerializeField] float _height;
    [SerializeField] float _lenght;

    void Start () {
        //Brick scale must be (1, 0.5, 0.5)
        //Remeber to add a rigidbody component
        for (int lenght = 0; lenght < _lenght; lenght++)
        {
            for (int height=0; height<_height; height++)
            {
                GameObject brick = Instantiate(prefab) as GameObject;
                float offset = height % 2;
                brick.transform.position = new Vector3(transform.position.x + lenght + offset/2, transform.position.y + height, transform.position.z);
            }
        }
	}
}
