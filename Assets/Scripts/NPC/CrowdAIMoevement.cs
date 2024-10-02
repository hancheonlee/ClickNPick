using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrowdAIMoevement : MonoBehaviour
{
    [SerializeField] Transform waypoint;
    NavMeshAgent agent;
    public float stoppingDistance = 1f;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        agent.SetDestination(waypoint.position);

        if (Vector3.Distance(agent.transform.position, waypoint.position) <= stoppingDistance)
        {
            OnReachedWaypoint();
        }

    }

    private void OnReachedWaypoint()
    {
        Destroy(gameObject);
    }
}
