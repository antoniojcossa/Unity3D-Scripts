using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour {
    //When attaching this script to a player, dont forget to add character controller component and adjust the jumpFallOff curve animation(which is basically the force added to the jump over time)
	//Do not attach a rigidbody to the character
	//In time i will update this script to explain how everything works, but for now, just enjoy!

    [Header("Walk Variables")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float defWalkSpeed;
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    float horiInput;
    float vertInput;
    private float movementSpeed;
    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [Header("Run variables")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;
    [SerializeField] private float runFOV, walkFOV;
    [SerializeField] private float FOVSwitchTime = 2;
    private bool isRunning;

    [Header("Jump variables")]
    [SerializeField] private AnimationCurve jumpFalloff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode JumpKey;
    private bool isJumping;

    [Header("Crouch variables")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float standardHeight;
    [SerializeField] private float crouchHeight;
    private bool isCrouching;
    [SerializeField] private KeyCode crouchKey;

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

    private void PlayerMovement()
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