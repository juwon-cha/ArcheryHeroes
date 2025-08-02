using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class TutorialDungeonRoom : MonoBehaviour
{
    [Header("몬스터 설정")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("다음 문 설정")]
    [SerializeField] private TutorialDungeonDoor door;

    [Header("플레이어 스폰 위치")]
    [SerializeField] private Transform playerSpawnPosition;

    private TutorialUI tutorialUI;


    private GameObject player;
    private PlayerController playerController;

    private bool isRight;
    private bool isSpawned;

    private List<GameObject> spawnedMonsters = new();

    private void Start()
    {
        player = GameManager.Instance.Player;
        playerController = player.GetComponent<PlayerController>();
        tutorialUI = FindObjectOfType<TutorialUI>();
    }
    private void Update()
    {
        CheckPlayerPosition();

        if (playerController.isInterAct && !isSpawned)
        {
            SpawnEnemy();
            playerController.isInterAct = false;
        }
        CheckClearCondition();
    }
    private void CheckPlayerPosition()
    {
        Vector3 pivot = Vector2.zero;
        if(player.transform.position.x > pivot.x)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPoint;
        if (isRight)
            spawnPoint = new Vector3(-5, 0, 0);
        else
            spawnPoint = new Vector3(5, 0, 0);

            var enemy = ObjectPoolingManager.Instance.Get(enemyPrefab, spawnPoint, Quaternion.identity);
        spawnedMonsters.Add(enemy);
        isSpawned = true;
    }


    // 플레이어 생성 위치를 반환한다.
    public Transform GetPlayerSpawnPoint()
    {
        // 방 진입 시 플레이어가 생성될 위치를 구한다.
        // 방마다 다를 수 있기 때문에
        if (playerSpawnPosition != null)
            return playerSpawnPosition;
        else
        {
            UnityEngine.Debug.Log("PlayerSpawnPostion이 지정되지 않았습니다.");
            return this.transform;
        }
    }

    // 방에 남은 몬스터를 체크하는 메서드
    private void CheckClearCondition()
    {
        if(tutorialUI.isStop)
        {
            door.OpenDoor();
        }
        Vector3 pivot = new Vector3(0, 4.5f, 0);
        if (player.transform.position.y > pivot.y)
        {
            for (int i = spawnedMonsters.Count - 1; i >= 0; i--)
            {
                ObjectPoolingManager.Instance.Return(spawnedMonsters[i]);
            }
            tutorialUI.next.SetActive(false);
            spawnedMonsters.Clear();
        }
            
    }
}
