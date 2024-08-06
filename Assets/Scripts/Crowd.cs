using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crowd : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject waypoint;
    public GameObject[] allWaypoints;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        FindWaypoint();
        waypoint = allWaypoints[Random.Range(0, allWaypoints.Length)];
    }


    private void Update()
    {
        navMeshAgent.SetDestination(waypoint.transform.position);

        if (Vector3.Distance(this.transform.position, waypoint.transform.position) <= 1.2f)
        {
            waypoint = allWaypoints[Random.Range(0, allWaypoints.Length)];
        }
    }

    public void FindWaypoint()
    {
        if (waypoint != null)
        {
            waypoint.transform.tag = "Waypoint";
        }
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoint = allWaypoints[Random.Range(0, allWaypoints.Length)];
    }

        /*    private void Update()
            {
                if (waypoint != null)
                {
                    if (Vector3.Distance(this.transform.position, waypoint.transform.position) <= 2f)
                    {
                        FindWaypoint();
                    }
                }
            }

            public void FindWaypoint()
            {
                if (waypoint != null)
                {
                    waypoint.transform.tag = "Waypoint";
                }
                allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
                waypoint = allWaypoints[Random.Range(0, allWaypoints.Length)];
                waypoint.transform.tag = "Untagged";

                navMeshAgent.SetDestination(waypoint.transform.position);
            }*/
    }
