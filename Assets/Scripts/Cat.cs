using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{
    public float moveRadius = 10f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;


    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(MoveRoutine());
    }

    public void MoveToRandomPoint()
    {
        Vector3 randomPos = Random.insideUnitSphere * moveRadius;
        randomPos += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, moveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            MoveToRandomPoint();
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, moveRadius);
    }
}
