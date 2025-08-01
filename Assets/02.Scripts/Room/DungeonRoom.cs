using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoom : MonoBehaviour
{
    [Header("몬스터 설정")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Tilemap enemySpawnPoint;
    [SerializeField] private int minEnemies = 3;
    [SerializeField] private int maxEnemies = 7;
    [SerializeField] private Transform enemyParent;

    [Header("다음 문 설정")]
    [SerializeField] private DungeonDoor door;

    [Header("플레이어 스폰 위치")]
    [SerializeField] private Transform playerSpawnPosition;

    // 현재 방에서 살아있는 몬스터 리스트
    private List<EnemyController> activeEnemies = new List<EnemyController>();

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        // 몬스터 스폰 영역이 없거나 몬스터 프리팹에 몬스터가 없다면
        if (enemySpawnPoint == null || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("몬스터 스폰 영역 또는 프리팹이 지정되지 않았습니다.");
            //CheckClearCondition();
            return;
        }

        // 플레이어의 위치를 받아온다.
        // EnemyController에서 Init에 넣어주기 위해
        Transform playerTransform = GameManager.Instance.Player.transform;

        // 랜덤한 마리수의 몬스터 생성
        int enemyCount = Random.Range(minEnemies, maxEnemies);

        List<Vector2> possibleSpawnPoints = new List<Vector2>();
        enemySpawnPoint.CompressBounds();
        BoundsInt tileBounds = enemySpawnPoint.cellBounds;

        for (int x = tileBounds.xMin; x < tileBounds.xMax; x++)
        {
            for (int y = tileBounds.yMin; y < tileBounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (enemySpawnPoint.HasTile(tilePosition))
                {
                    // 해당 위치가 장애물에 막혀있지 않은지도 추가로 확인한다.
                    Vector2 worldPos = enemySpawnPoint.GetCellCenterWorld(tilePosition);
                    if (IsPositionValid(worldPos, 1.0f))
                    {
                        possibleSpawnPoints.Add(worldPos);
                    }
                }
            }
        }

        // 만약 스폰 가능한 위치가 하나도 없다면, 즉시 클리어 처리
        if (possibleSpawnPoints.Count == 0)
        {
            CheckClearCondition();
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            // 유효한 위치 리스트에서 하나 가져온다
            Vector2 spawnPosition = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];

            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemyParent);
            EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                enemyController.Init(playerTransform, this);
                activeEnemies.Add(enemyController);
            }
        }
    }

    // 해당 위치에 장애물이 있는지 확인한다.
    private bool IsPositionValid(Vector2 position, float checkSize)
    {
        // 레이어 Obstacle을 int로 반환한다
        int obstacleLayerMask = LayerMask.GetMask("Obstacle");
        // position에 범위 checkSize만큼 확인해서 obstacleLayerMask 레이어의 오브젝트가 있는지 확인한다.
        Collider2D blockingCollider = Physics2D.OverlapCircle(position, checkSize, obstacleLayerMask);
        return blockingCollider == null; // null이라는 것은 해당 범위에 장애물이 없는 소환 가능한 공간이라는 것이다.
    }

    // 몬스터를 잡으면 이 메서드도 불러야 한다.
    public void OnEnemyKill(EnemyController enemy)
    {
        // 몬스터가 해당 방에 포함된 몬스터라면
        if (activeEnemies.Contains(enemy))
        {
            // 리스트에서 삭제한다.
            activeEnemies.Remove(enemy);
        }
        // 몬스터를 잡으면 방의 클리어 상태를 확인한다.
        CheckClearCondition();
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
            Debug.Log("PlayerSpawnPostion이 지정되지 않았습니다.");
            return this.transform;
        }
    }

    // 방에 남은 몬스터를 체크하는 메서드
    private void CheckClearCondition()
    {
        if(activeEnemies.Count == 0)
        {
            door.OpenDoor();
        }
    }
}
