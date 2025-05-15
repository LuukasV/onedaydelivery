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
    public Transform dogGrabPoint;
    [SerializeField] private float dogThrowForce;

    private NavMeshAgent agent;
    [Header("Player detection and NPC movement")]
    [SerializeField] private Transform playerTransform;
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
    /// Update is called once per frame.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("PlayerFirstType").transform;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // either i need to change how to get into envinromnetview or i change the enviroment view by creating a else if for the for loop. For loop notices player
        // so else if it doenst see a player. If ican get an out of sight variable from enviroentavile. I could use that to set the chas or patrol.
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
    /// NPC starts chasing the player. Destination set as player location and new chase speed is set. If NPC loses sight of player, NPC will stop and wait.
    /// </summary>
    private void getAfter()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false;

        agent.SetDestination(playerTransform.position);

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

        // if walkPointSet is true, go there.
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
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
    /// Npc sight and player detection.
    /// </summary>
    //void EnviromentView()
    //{
    //    // if player is within a certain radius, information about it will be stored within players array.
    //    Collider[] players = Physics.OverlapSphere(transform.position, viewRadius, whatIsPlayer);
    //    foreach (Collider playerCollider in players)
    //    {
    //        Vector3 dirToPlayer = (playerCollider.transform.position - transform.position).normalized;
    //        float angle = Vector3.Angle(transform.forward, dirToPlayer);

    //        // if player is within a certain angle, he will be detected
    //        if (angle < viewAngle / 2f)
    //        {
    //            //Debug.Log("I see within angle and radius");
    //            float dstToPlayer = Vector3.Distance(transform.position, playerCollider.transform.position);

    //            if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
    //            {
    //                m_PlayerInRange = true;
    //                m_IsPatrol = false;
    //            }
    //            else
    //            {
    //                m_PlayerInRange = false;
    //            }
    //        }

    //        // if player moves out of a certain radius or angle, npc will lose sight of player.
    //        if ((Vector3.Distance(transform.position, playerCollider.transform.position) > viewRadius) || (Vector3.Angle(transform.forward, dirToPlayer) > viewAngle / 2))
    //        {
    //            //Debug.Log("lost sight");

    //            m_PlayerInRange = false;
    //        }
    //        if (m_PlayerInRange)
    //        {
    //            m_PlayerPosition = playerCollider.transform.position;
    //        }
    //    }
    //}

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
            if (angle < viewAngle / 2f& !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
            {
                //Debug.Log("I see within angle and radius");
                m_PlayerInRange = true;
                m_IsPatrol = false;
                m_PlayerPosition = playerCollider.transform.position;
                break;
            }
        }
    }

    /// <summary>
    /// Starts coroutine if npc has grabbed a package from player.
    /// </summary>
    /// <param name="stolenPackage"> A package that was stolen from the player </param>
    public void runAwayAndThrow(ObjectGrabbable stolenPackage)
    {
        StartCoroutine(SmoothTurnAndThrow(stolenPackage));
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
    }

    /// <summary>
    /// Returns information about the NPC that collided with player.
    /// </summary>
    public AI_DogSimple npcInformation()
    {
        return this;
    }
}
