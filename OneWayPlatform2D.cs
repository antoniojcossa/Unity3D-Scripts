using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform2D : MonoBehaviour {

	//You can jump on the platform from below, as well as fall through by holding the S key
	//The waitValue variable determines how long you have to press the S key in order to fall through the platform
	//Select the platform you want to use, make sure it has a 2d box collider and check the "used by effector box" and then attach this script to it
	//Add a new component called "Platform Effector 2d"
    private PlatformEffector2D effector;
    private float waitTime;
    public float waitValue = 0.5f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            waitTime = waitValue;
            effector.rotationalOffset = 0;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if(waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = waitValue;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
