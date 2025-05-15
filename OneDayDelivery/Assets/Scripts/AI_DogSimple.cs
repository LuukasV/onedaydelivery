using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

/// <summary>
/// NPC script intended for guard dog use. Dog detects and chases player. If player is caught, steals package and throws it.
/// </summary>
public class AI_DogSimple : MonoBehaviour
{
    //Set the area where the NPC patrols
    [Header("Patrol area coordinates")]
    [SerializeField] private float xLargePatrolPointRange;
    [SerializeField] private float xSmallPatrolPointRange;
    [SerializeField] private float zLargePatrolPointRange;
    [SerializeField] private float zSmallPatrolPointRange;

    // Chase variables
    [Header("movement variables")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float startWaitTime;
    [SerializeField] private float currentWaitTime;
    [SerializeField] private float dogPushPlayerStr;
    [SerializeField] private float dogPushDisableMovementTime;


    public Transform dogGrabPoint;
    [SerializeField] private float dogThrowForce;
    private bool hasStolenPackage = false;

    private float lastSeenTime = Mathf.NegativeInfinity;
    [SerializeField] private float playerMemoryDuration = 1.5f;

    private NavMeshAgent agent;
    [Header("Player detection and NPC movement")]
    private Transform playerTransform;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    //Environment view
    [Header("NPC sight and detection")]
    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask obstacleMask;
    bool m_PlayerInRange;
    bool m_IsPatrol;
    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    // Patrol variables
    [Header("Patrol walkPoint for debugging")]
    public Vector3 walkPoint;
    bool walkPointSet;

    [Header("Cone color for debugging")]
    public Color coneColor = Color.green; // Color for the cone for debugging in UNITY.

    /// <summary>
    /// Draws a cone that corresponds to the NPC field of vision. Only visible in UNITY. Intended use is for debugging purposes.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Set the color of the cone
        Gizmos.color = coneColor;

        // Draw the central forward line (AI's forward direction)
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewRadius);

        // Draw the left boundary of the cone
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);

        // Draw the right boundary of the cone
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        // Optionally, draw a small sphere at the center of the cone to represent the AI's position
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        currentWaitTime = startWaitTime;
    }

    /// <summary>
    /// Set variables needed for player interaction and NPC movement.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("PlayerFirstType").transform;
    }

    /// <summary>
    /// Runs dog vision and detection method and patrol and chase behaviours.
    /// </summary>
    void Update()
    {
        EnviromentView();

        if (m_IsPatrol)
        {
            Patroling();
        }

        else
        {
            getAfter();
        }
    }

    /// <summary>
    /// NPC starts chasing the player. Destination set as player location and new chase speed is set. If NPC loses sight of player, NPC will stop and wait, and NPC variables are set to patrol state.
    /// </summary>
    private void getAfter()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false;

        agent.SetDestination(playerTransform.position);

        //Debug logs from debugging purposes if necessary.
        //Debug.Log("DOG Agent destination: " + agent.destination);
        //Debug.Log("DOG Remaining distance: " + agent.remainingDistance);

        // if player isn't visible stop and continue patrolling.
        if (m_PlayerInRange == false)
        {
            agent.isStopped = true;
            agent.speed = 0;
            currentWaitTime -= Time.deltaTime;

            if (currentWaitTime <= 0)
            {
                agent.isStopped = false;
                agent.speed = patrolSpeed;
                m_IsPatrol = true;
                currentWaitTime = startWaitTime;
            }
        }
    }

    /// <summary>
    /// Runs a method for randomizing coordinates on game map and setting them to be the movement destination for the NPC.
    /// </summary>
    private void Patroling()
    {
        // if walkPointSet is false, search one and set it to true.
        if (!walkPointSet) SearchWalkPoint();

        // if walkPointSet is true, set it as destination and go there.
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    /// <summary>
    /// Randomizes x and z coordinates within set number ranges. If coordinates are within the gameplay area, a boolean for NPC movement is set to true.
    /// </summary>
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(zSmallPatrolPointRange, zLargePatrolPointRange);
        float randomx = Random.Range(xSmallPatrolPointRange, xLargePatrolPointRange);

        walkPoint = new Vector3(randomx, transform.position.y, randomZ);
        if (Physics.Raycast(walkPoint + Vector3.up * 2f, -transform.up, 4f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    /// <summary>
    /// NPC detection and sight method. If player is within set radius and angle, the NPC detects the player. 
    /// </summary>
    void EnviromentView()
    {
        m_PlayerInRange = false;

        // if player is within a certain radius, information about it will be stored within players array.
        Collider[] players = Physics.OverlapSphere(transform.position, viewRadius, whatIsPlayer);
        foreach (Collider playerCollider in players)
        {
            Vector3 dirToPlayer = (playerCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            float dstToPlayer = Vector3.Distance(transform.position, playerCollider.transform.position);

            // if player is within a certain angle, he will be detected
            if (angle < viewAngle / 2f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
            {
                //Debug.Log("I see within angle and radius");
                m_PlayerInRange = true;
                lastSeenTime = Time.time;
                m_IsPatrol = false;
                m_PlayerPosition = playerCollider.transform.position;
                break;
            }
        }
    }

    /// <summary>
    /// Starts coroutine to throw a package if npc has grabbed a package from player. Pushes the player away from the NPC.
    /// </summary>
    /// <param name="stolenPackage"> A package that was stolen from the player </param>
    public void runAwayAndThrow(ObjectGrabbable stolenPackage)
    {
        //if (!m_PlayerInRange || hasStolenPackage || stolenPackage == null)
        //{
        //    Debug.Log("Dog can't steal. In range: " + m_PlayerInRange + " | Already stole: " + hasStolenPackage);
        //    return;
        //}

        //Debug.Log(m_PlayerInRange + " " + hasStolenPackage);
        //if (!m_PlayerInRange || hasStolenPackage) return;
        //hasStolenPackage = true;
        if (!CanSteal() || stolenPackage == null) return;

        hasStolenPackage = true; // 
        StartCoroutine(SmoothTurnAndThrow(stolenPackage));
        PushPlayerAway();
    }


    /// <summary>
    /// If player has a package, he is pushed away and his movement controls are disabled for a set time.
    /// </summary>
    private void PushPlayerAway()
    {
        // Check if the player has a Rigidbody component
        Rigidbody playerRb = playerTransform.GetComponent<Rigidbody>();
        PlayerMovement playerMovement = playerTransform.GetComponent<PlayerMovement>();

        if (playerRb != null && playerMovement != null)
        {
            playerMovement.canMove = false;
            StartCoroutine(EnableMovementAfterDelay(playerMovement, dogPushDisableMovementTime)); // 1 second delay

            // Calculate direction to push player away (from dog to player)
            Vector3 pushDirection = playerTransform.position - transform.position;
            pushDirection.y = 0.75f; // Keep the push direction horizontal (ignoring vertical force)

            // Apply a force to push the player away
            playerRb.AddForce(pushDirection.normalized * dogPushPlayerStr, ForceMode.Impulse); // Adjust the "10f" to control the strength of the push
        }
    }

    /// <summary>
    /// Enables movement after a set delay.
    /// </summary>
    private IEnumerator EnableMovementAfterDelay(PlayerMovement playerMovement, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.canMove = true;
    }

    /// <summary>
    /// When NPC has grabbed a package from player, the NPC will rotate 180 degrees and throw stolen package.
    /// </summary>
    /// <param name="package"> A package that was stolen from the player </param>
    private IEnumerator SmoothTurnAndThrow(ObjectGrabbable package)
    {
        // Start and end rotation (rotate 180 degrees on Y axis)
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180f, transform.eulerAngles.z);

        float rotationDuration = 1f; // Time in seconds to complete the turn
        float elapsed = 0f;

        // Smoothly rotate NPC over time
        while (elapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final rotation is exactly 180° rotated
        transform.rotation = endRot;

        yield return new WaitForSeconds(0.1f); // Short delay for realism

        package.Drop();
        yield return null; // Wait one frame so Rigidbody is initialized
        // Throw upward-forward
        Vector3 throwDirection = transform.forward + Vector3.up * 0.5f;
        package.Throw(dogThrowForce, throwDirection.normalized);
        hasStolenPackage = false;
    }

    /// <summary>
    /// Returns information about the NPC that collided with player.
    /// </summary>
    public AI_DogSimple npcInformation()
    {
        return this;
    }

    /// <summary>
    /// Checks conditions for stealing a package. Returns either true or false, if a package can be stolen.
    /// </summary>
    public bool CanSteal()
    {
        bool recentlySawPlayer = Time.time - lastSeenTime <= playerMemoryDuration;
        return !hasStolenPackage && recentlySawPlayer;
    }
}
