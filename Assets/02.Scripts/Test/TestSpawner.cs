using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    public float spawnRate = 1f;
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0, spawnRate);
    }

    void Spawn()
    {
        if (spawnPrefab == null) return;

        ObjectPoolingManager.Instance.Get(spawnPrefab, transform.position, Quaternion.identity);
    }
}
