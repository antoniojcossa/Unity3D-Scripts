using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform2D : MonoBehaviour {

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
