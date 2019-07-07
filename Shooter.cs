using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
	//This script fires a certain gameobject as a rigidbody
	//Attach the script to a first person camera
	//Also create an empty gameobject and place it where you want the bullets to come from
    [SerializeField] Rigidbody projectile; //the gameobject you want to fire
    [SerializeField] Transform shotPos; //This is the position of the empty gameobject you created
    [SerializeField] float shotForce; //The force of the shot

    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody shot = Instantiate(projectile, shotPos.position, shotPos.rotation) as Rigidbody;
            shot.AddForce(shotPos.forward * shotForce);
        }
	}
}
