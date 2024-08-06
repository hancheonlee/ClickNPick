using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public GameObject[] crowdPrefab;
    public int crowdSize = 10;
    public Transform[] spawnPoints;
    public float spawnRate = 0.5f;

    private List<GameObject> crowd = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnCrowd());
    }

    IEnumerator SpawnCrowd()
    {
        for (int i = 0; i < crowdSize; i++)
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject randomCrowds = crowdPrefab[Random.Range(0, crowdPrefab.Length)];
            GameObject newHuman = Instantiate(randomCrowds, randomSpawnPoint.position, Quaternion.identity);
            crowd.Add(newHuman);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
