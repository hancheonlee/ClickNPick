using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public GameObject crowdPrefab;
    public int crowdSize = 15;
    public Transform[] spawnPoints;
    public float spawnRate = 1f;

    private List<GameObject> crowd = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnCrowd());
    }

    IEnumerator SpawnCrowd()
    {
        for (int i = 0; i < crowdSize; i++)
        {
            // Select a random spawn point from the array
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the prefab at the random spawn point
            GameObject newHuman = Instantiate(crowdPrefab, randomSpawnPoint.position, Quaternion.identity);

            // Add the new human to the crowd list
            crowd.Add(newHuman);

            // Wait for the specified spawn rate before spawning the next one
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
