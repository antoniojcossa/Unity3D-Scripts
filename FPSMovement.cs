using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour {
    //When attaching this script to a player, dont forget to add character controller component and adjust the jumpFallOff curve animation(which is basically the force added to the jump over time)
	//Do not attach a rigidbody to the character

    [Header("Walk Variables")]
    [SerializeField] private float walkSpeed; //This is your walk speed (it changes when you crouch)
    [SerializeField] private float defWalkSpeed; //This is your normal walk speed
    [SerializeField] private string horizontalInputName; //This is the name of your horizontal input in unity (you can check its name in Edit -> Project Settings -> input)
    [SerializeField] private string verticalInputName; //This is the name of your vertical input in unity (you can check its name in Edit -> Project Settings -> input)
    float horiInput;
    float vertInput;
    private float movementSpeed; //This is your movement speed (it changes when you sprint)
    private float slopeForce;
    private float slopeForceRayLength;

    [Header("Run variables")]
    [SerializeField] private float runSpeed; //This is your running speed
    [SerializeField] private float runBuildUpSpeed; //This indicates how fast you go from walking to running, or vice-versa
    [SerializeField] private KeyCode runKey; //This is the key you press to run
    [SerializeField] private float runFOV, walkFOV; //Use these two variables to change the Field of View when your character is running or walking
    [SerializeField] private float FOVSwitchTime = 2; //This indicates how fast it changes the field of view
    private bool isRunning;

    [Header("Jump variables")]
    [SerializeField] private AnimationCurve jumpFalloff; //This curve indicates the amount of force added to the jump animation over time
    [SerializeField] private float jumpMultiplier; //This is the initial jump force
    [SerializeField] private KeyCode JumpKey; //This is your jump key
    private bool isJumping;

    [Header("Crouch variables")]
    [SerializeField] private float crouchSpeed; //This is your crouch walking speed
    [SerializeField] private float standardHeight; //This is your height when standing
    [SerializeField] private float crouchHeight; //This is your height when crouching
    private bool isCrouching;
    [SerializeField] private KeyCode crouchKey; //This is your crouch key

    private CharacterController charController;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        walkSpeed = defWalkSpeed;
        isJumping = false;
        isRunning = false;
        isCrouching = false;
        Camera.main.fieldOfView = 60.0f;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement() //This method works on everything involving movement
    {
        horiInput = Input.GetAxis(horizontalInputName);
        vertInput = Input.GetAxis(verticalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horiInput * 0.85f;

        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if((vertInput != 0 || horiInput != 0) && OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }

        crouchInput();
        runInput();
        JumpInput();
    }


    private bool OnSlope()
    {
        if (isJumping)
        {
            return false;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    private void runInput()
    {
        if (Input.GetKey(runKey) && vertInput > 0 && !isJumping && isCrouching)
        {
            isCrouching = false;
        }

        if (Input.GetKey(runKey) && vertInput > 0 && !isJumping && !isCrouching)
        {
            isRunning = true;
            setMovementSpeed(isRunning);
        }
        else
        {
            isRunning = false;
            setMovementSpeed(isRunning);
        }
    }

    private void setMovementSpeed(bool _isRunning)
    {
        if (isRunning)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, runFOV, Time.deltaTime * FOVSwitchTime);
        }
        else
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, walkFOV, Time.deltaTime * FOVSwitchTime);
        }
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(JumpKey) && !isJumping && charController.isGrounded)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90f;
        float airTime = 0f;

        do
        {
            float jumpForce = jumpFalloff.Evaluate(airTime);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            airTime += Time.deltaTime;
            if ((charController.collisionFlags & CollisionFlags.Above) != 0)
            {
                break;
            }

            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45f;
        isJumping = false;
    }

    private void crouchInput()
    {
        if (Input.GetKeyDown(crouchKey) && !isJumping && !isRunning)
        {
            isCrouching = !isCrouching;
        }
        crouch(isCrouching);
    }

    private void crouch(bool _isCrouching)
    {
        if (_isCrouching)
        {
            charController.height = crouchHeight;
            walkSpeed = crouchSpeed;
        }
        else
        {
            charController.height = standardHeight;
            walkSpeed = defWalkSpeed;
        }
    }
}