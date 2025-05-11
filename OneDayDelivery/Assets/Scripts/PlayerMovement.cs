using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player movement script that allows the player to control a character. Adds mobility to character.
/// If movement is jittery. Test interpolate = None and collision detection = Discrete changes.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    // Left, right, forward and back player movement input.
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    // Variables determining player movement speed and drag.
    public float moveSpeed;
    public float groundDrag;
    public float airDrag;
    public float walkSpeed;

    // Jump variables.
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public float groundDistance = 0.4f;

    // TEST�NG NEW JUMP
    // Tester for gravitifier timer. DELETE WHEN NOT NECESSARY.
    //public float waitTimeForCounterGrav;
    public float counterGravStr;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode restartKey = KeyCode.F1;
    public KeyCode walkKey = KeyCode.LeftShift;

    // Variables used for registering player input.
    bool jumpingInput;
    bool restartInput;
    bool walkInput;
    

    [Header("Changable values for play testing")]
    [SerializeField] private float throwForceIncrAmount;
    [SerializeField] private float jumpForceIncrAmount;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // Variables for character physics.
    // serialize field here private
    public Transform orientationAndGroundCheck;
    Rigidbody rb;

    [Header("Variable change access")]
    // how does this work in practice? What variable type. with player pickup drop type.
    [SerializeField] private Transform accessToThrowForce;

    [Header("Variable for disabling moving")]
    public bool canMove = true; // Boolean for disabling movement


    /// <summary>
    /// Runs method that takes in player input. Also, checks if player character is on the ground or not.
    /// </summary>
    private void Update()
    {
        grounded = Physics.CheckSphere(orientationAndGroundCheck.position, groundDistance, whatIsGround);

        if (canMove)
        {
            MyInput();
            SpeedControl();
        }

        if (restartInput)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }

        // Player jumps
        if (jumpingInput && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            // Test for timed antigravity outside of fixed update time stamp. DELETE WHEN NOT NECESSARY.
            //StartCoroutine (Gravitifier());

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        jumpingInput = false;
        //Debug.Log(rb.linearVelocity.x);
        //Debug.Log(rb.linearVelocity.magnitude);
        //Debug.Log(rb.linearVelocity.y);

        if (!grounded)
        {
            ApplyCounterGravity();
        }

    }

    /// <summary>
    /// Antigravity method to make the player character fall faster when !grounded.
    /// </summary>
    private void ApplyCounterGravity()
    {
        // Apply counter gravity to simulate more natural fall or counter gravity effect
        rb.AddForce(-transform.up * counterGravStr * Time.fixedDeltaTime, ForceMode.Force);
    }

    /// <summary>
    /// Sets variables needed for player movement and rotation.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        ResetJump();

        //We determine, if any upgrades are in effect
        if (GameData.item2Active)
        {
            ActivateJumpBuff();
        }
    }

    /// <summary>
    /// Takes in player input for jumps and body movement.
    /// </summary>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpingInput = jumpingInput || Input.GetKey(jumpKey);
        restartInput = Input.GetKey(restartKey);
        walkInput = Input.GetKey(walkKey);
    }

    /// <summary>
    /// Moves player based on orientation and inputs
    /// </summary>
    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientationAndGroundCheck.forward * verticalInput + orientationAndGroundCheck.right * horizontalInput;
        // on ground
        if (grounded && walkInput == false)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (grounded && walkInput == true)
        {
            rb.AddForce(moveDirection.normalized * walkSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    /// <summary>
    /// Controls characters speed. Makes sure it doesn't go faster than set speed.
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    // HARD CAP ON MOVESPEED, EVEN DURING JUMPING. USE THIS OR speedControl
    private void SpeedControls()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Jump mechanics for player's character.
    /// </summary>
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // TESTABLE METHOD FOR TIMED ANTIGRAVITY
    //private IEnumerator Gravitifier()
    //{
    //    yield return new WaitForSeconds(waitTimeForCounterGrav);

    //    //while (!grounded)
    //    //{
    //    //    rb.AddForce(-transform.up * counterGravStr, ForceMode.Force); // Apply constant downward force
    //    //    yield return new WaitForSeconds(0.05f); // Adjust frequency of force application
    //    //}

    //    while (!grounded)
    //    {
    //        rb.AddForce(-transform.up * counterGravStr * Time.fixedDeltaTime, ForceMode.Force);
    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    /// <summary>
    /// Method that resets player jump cooldown to make the character ready to jump.
    /// </summary>
    private void ResetJump()
    {
        readyToJump = true;
    }

    /// <summary>
    /// Detects objects that the player collides with.
    /// </summary>
    /// <param name="other"> A collider that checks what the player picks up </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("jumpBuff"))
        {
            jumpForce += jumpForceIncrAmount;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("throwBuff"))
        {
            accessToThrowForce.GetComponent<PlayerPickUpDrop>().throwForceBooster(throwForceIncrAmount);
            other.gameObject.SetActive(false);
        }
    }

    //Activates Throw Buff
    private void ActivateThrowBuff()
    {
        accessToThrowForce.GetComponent<PlayerPickUpDrop>().throwForceBooster(throwForceIncrAmount);
        Debug.Log("Player has a throwbuff activated");
    }

    //Activates Jump Buff
    private void ActivateJumpBuff()
    {
        jumpForce += jumpForceIncrAmount;
        Debug.Log("Player has a jumpbuff activated");
    }
}
