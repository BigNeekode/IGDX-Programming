using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnRate = 2f;

    void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    IEnumerator SpawnObstacle()
    {
        while (true)
        {
            Instantiate(obstaclePrefab, transform.position + new Vector3(0, Random.Range(1, -1), 0), Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
