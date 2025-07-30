using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // TEMP Player
    [SerializeField] private GameObject playerPrefab;
    public GameObject Player { get; private set; }

    void Start()
    {
        Player = playerPrefab;

        // Test
        EnemyManager.Instance.StartWave(5);
    }

    void Update()
    {
        
    }

    public void EndOfWave()
    {

    }
}
