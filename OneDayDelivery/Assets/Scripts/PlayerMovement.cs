using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Player movement script that allows the player to control a character. Adds mobility to character.
// If movement is jittery. Test interpolate = None and collision detection = Discrete changes.
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
    public float FallMultiplier;

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
    // serialize field here rpivate
    public Transform orientationAndGroundCheck;
    Rigidbody rb;

    [Header("Variable change access")]
    // how does this work in practice? What variable type. with player pickup drop type.
    [SerializeField] private Transform accessToThrowForce;


    // Runs method that takes in player input. Also, checks if player character is on the ground or not.
    private void Update()
    {
        grounded = Physics.CheckSphere(orientationAndGroundCheck.position, groundDistance, whatIsGround);

        MyInput();
        //ORIGINAL SPEED CONTROL PLACEMENT
        SpeedControl();

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
            // ORIGINAL
            rb.linearDamping = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        // Player jumps MAYBE REMOVE THIS TESTING IN FIXED UPDATE INSTEAD OF UDATE.
        if (jumpingInput && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
        //remove and test?
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        jumpingInput = false;
        //Debug.Log(rb.linearVelocity.x);
        //Debug.Log(rb.linearVelocity.magnitude);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * FallMultiplier * Time.deltaTime;
        }

    }

    // Sets variables needed for player movement and rotation.
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
    }

    // Takes in player input for jumps and body movement.
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpingInput = jumpingInput || Input.GetKey(jumpKey);
        restartInput = Input.GetKey(restartKey);
        walkInput = Input.GetKey(walkKey);
    }

    // Moves player based on orientation and inputs
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

    // Controls characters speed. Makes sure it doesn't go faster than set speed.
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

    // Jump mechanics for player's character.
    private void Jump()
    {
        //reset y velocity ?
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    // Jumping cooldown.
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
}
