using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Created by Cindy Shi.
/// Allow buggies to patrol among waypoints.
/// </summary>

public class BuggyPatrol : MonoBehaviour
{
    //Dictates whether the agent waits on each node
    [SerializeField] bool patrolWaiting;
    //Total time we wait at each node
    [SerializeField] float totalWaitTime = 3f;
    //Probability of switiching direction
    [SerializeField] float switchProbability = 0.2f;
    //list of patrol nodes
    [SerializeField] List<Waypoint> patrolPoints;

    //Private variables for base bebavior
    NavMeshAgent navMeshAgent;
    int currentPatrolIndex;
    bool travelling, waiting, patrolForward;
    float waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
            Debug.Log("null nav mesh agent");
        else
        {
            if (patrolPoints != null && patrolPoints.Count >= 2)
            {
                currentPatrolIndex = 0;
                SetDestination();
            }
            else
                Debug.Log("insufficient patrol points");
                

      
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check if we are close to the destination
        if(travelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            travelling = false;

            //waiting on the patrol point
            if (patrolWaiting)
            {
                waiting = true;
                waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer>= totalWaitTime)
            {
                waiting = false;
                ChangePatrolPoint();
                SetDestination();
            }
        }
    }



    private void SetDestination()
    {
        if(patrolPoints!= null)
        {
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
    }
    /// <summary>
    /// Selects a new patrol point in the available list
    /// but also with a small probability allows for us to move forward or backwards
    /// </summary>
    private void ChangePatrolPoint()
    {
        if(UnityEngine.Random.Range(0f,1f) <= switchProbability)
        {
            patrolForward = !patrolForward;
      
        }

        if (patrolForward)
        {
            currentPatrolIndex = (currentPatrolIndex + 1 ) % patrolPoints.Count;

        }
        else
        {
            currentPatrolIndex = (currentPatrolIndex + patrolPoints.Count - 1) % patrolPoints.Count;

        }
    }
}
