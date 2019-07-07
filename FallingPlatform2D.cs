using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to the platform you want to use
//Make sure it has two box colliders, make one of the colliders a trigger and make it stick out of the platform a little on the top(so that it activates once the player touches it)
public class FallingPlatform2D : MonoBehaviour {

    public bool isFalling = false;
    float downSpeed = 0.02f;
    bool fall = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.name == "Player")
        {
            fall = true;
        }
    }

    void Update()
    {
        if (fall == true)
        {
            isFalling = true;
            Destroy(gameObject, 5);
        }

        if (isFalling == true)
        {
            downSpeed += Time.deltaTime/1000;
            transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, transform.position.z);
        }
    }
}
