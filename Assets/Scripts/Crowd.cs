using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crowd : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject waypoint;
    public GameObject[] allWaypoints;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float fadeDuration = 2f;

    public float exclusionRange = 5f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;


        FindWaypoint();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }


    private void Update()
    {
        navMeshAgent.SetDestination(waypoint.transform.position);

        if (Vector3.Distance(this.transform.position, waypoint.transform.position) <= 2f)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    public void FindWaypoint()
    {
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        // Filter out waypoints that are within the exclusion range
        List<GameObject> validWaypoints = new List<GameObject>();
        foreach (GameObject wp in allWaypoints)
        {
            if (Vector3.Distance(this.transform.position, wp.transform.position) > exclusionRange)
            {
                validWaypoints.Add(wp);
            }
        }

        if (validWaypoints.Count > 0)
        {
            waypoint = validWaypoints[Random.Range(0, validWaypoints.Count)];
        }
        else
        {
            // If no valid waypoints are found (all are within the exclusion range), you might want to handle this case.
            Debug.LogWarning("No valid waypoints found outside the exclusion range!");
        }
    }
    private IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(gameObject);
    }

}
