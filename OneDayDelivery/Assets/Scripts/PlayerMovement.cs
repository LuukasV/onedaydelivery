using UnityEngine;

/// <summary>
/// Main programmer: Jussi Kolehmainen
/// Other progammers: Jari-Pekka Riihinen and Luukas Vuolle
/// Majority of the programming was done by the main programmer. Other programmers added a boolean and an if statment to disable movement and 
/// to check if any upgrades are in effect.
/// 
/// Player movement script that allows the player to control a character. Also gives functionality to increasing player's jump force and collision detection.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    // Left, right, forward and back player movement input.
    private float horizontalInput;
    private float verticalInput;
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
    public float counterGravStr;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode walkKey = KeyCode.LeftShift;

    // Variables used for registering player input.
    private bool jumpingInput;
    private bool walkInput;


    [Header("Increase amount of player variables at shop")]
    //[SerializeField] private float throwForceIncrAmount;
    [SerializeField] private float jumpForceIncrAmount;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    // Variables for character physics.
    public Transform orientationAndGroundCheck;
    private Rigidbody rb;

    [Header("Variable for access to child")]
    [SerializeField] private Transform accessToChild;

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

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        jumpingInput = false;

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

        // Determines, if any upgrades are in effect
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

    /// <summary>
    /// Jump mechanics for player's character.
    /// </summary>
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

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
    /// <param name="other"> Object that the player collided with </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("npcDog"))
        {
            accessToChild.GetComponent<PlayerPickUpDrop>().packageThievery(other.gameObject.GetComponent<AI_DogSimple>().npcInformation());
        }
    }

    /// <summary>
    /// Increases throw force of the player character.
    /// CODE NOT IMPLEMENTED IN GAME IN CURRENT VERSION.
    /// </summary>
    //private void ActivateThrowBuff()
    //{
    //    accessToChild.GetComponent<PlayerPickUpDrop>().throwForceBooster(throwForceIncrAmount);
    //}

    /// <summary>
    /// Increases jump force of the player character.
    /// </summary>
    private void ActivateJumpBuff()
    {
        jumpForce += jumpForceIncrAmount;
    }
}
