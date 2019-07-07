using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to a first person camera, this script goes well with my FPSMovement script
public class FPSCamera : MonoBehaviour {

	[SerializeField] private string mouseXInputName, mouseYInputName; //These two variables are the names of the mouse input X and Y (you can check these names under Edit->Project Settings->Input)
    [SerializeField] private float mouseSensitivity; //This variable controls how fast the camera looks around

    [SerializeField] private Transform PlayerBody; //This is the player in which the camera is attached to

    private float yAxisClamp; //This varible stops the from going all the way around the X axis

    private void Awake()
    {
        LockCursor(); //This command locks the mouse cursor in the middle of the screen
        yAxisClamp = 0;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxisRaw(mouseXInputName) * mouseSensitivity;
        float mouseY = Input.GetAxisRaw(mouseYInputName) * mouseSensitivity;

        yAxisClamp += mouseY;

        if (yAxisClamp > 90.0f)
        {
            yAxisClamp = 90f;
            mouseY = 0.0f;
        }
        else if (yAxisClamp < -90.0f)
        {
            yAxisClamp = -90f;
            mouseY = 0.0f;
        }

        transform.Rotate(Vector3.left * mouseY);
        PlayerBody.Rotate(Vector3.up * mouseX); //These two lines make sure that the player body turns with the camera along the Y axis
    }
}
