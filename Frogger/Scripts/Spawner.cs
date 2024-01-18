using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnerPrefab;

    public float minSpawnDelay = 1.0f;
    public float maxSpawnDelay = 4.0f;

    bool spawn = true;

    IEnumerator Start() {
        while (spawn) {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnObject();
        }
    }

    // Creates GO from spawner position
    private void SpawnObject() {
        Instantiate(spawnerPrefab, transform.position, transform.rotation);
    }
}
