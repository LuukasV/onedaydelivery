using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Programmer: Jussi Kolehmainen
/// NPC script for NPC's that move between different waypoints.
/// </summary>
public class AI_HarmlessWaypoint : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speedWalk;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int m_CurrentWaypointIndex;

    [Header("Wait time on points and display for current wait time")]
    [SerializeField] private float startWaitTime = 4;
    [SerializeField] private float currentWaitTime;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.speed = speedWalk;
        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        currentWaitTime = startWaitTime;
    }

    /// <summary>
    /// Runs methods that move the NPC to a  waypoint and checks if the NPC has reached a waypoint and is ready to move to another one.
    /// </summary>
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // When wait time is over, NPC will move to the next waypoint and reset wait timer.
            if (currentWaitTime <= 0)
            {
                NextPoint();
                Move(speedWalk);
                currentWaitTime = startWaitTime;
            }
            // NPC has reached the point and will wait for the defined wait time.
            else
            {
                Stop();
                currentWaitTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Calculates the next point in the waypoint array and sets NPC's movement destination to the next point and .
    /// </summary>
    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    /// <summary>
    /// Makes the NPC move to the next waypoint and sets the speed to the given speed variable.
    /// <param name="setSpeed"> Sets the NPC speed to the value defined by the programmer..</param>
    /// </summary>
    void Move(float setSpeed)
    {
        agent.isStopped = false;
        agent.speed = setSpeed;
    }

    /// <summary>
    /// Sets NPC's speed to 0 and stops the NPC.
    /// </summary>
    void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }
}