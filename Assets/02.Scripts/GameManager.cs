using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // TEMP Player
    [SerializeField] private GameObject playerPrefab;
    public GameObject Player { get; private set; }

    void Awake()
    {
        Player = playerPrefab;

        // 임시 플레이어 인스턴스 생성
        if (playerPrefab != null)
        {
            Player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Player.name = "Player";
        }
        else
        {
            Debug.LogError("GameManager에 Player Prefab이 지정되지 않았습니다!");
        }
    }

    void Update()
    {
        
    }

    public void EndOfWave()
    {

    }
}
