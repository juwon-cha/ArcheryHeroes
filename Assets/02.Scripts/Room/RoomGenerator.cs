using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    [Header("방의 영역 설정")]
    //[SerializeField] private BoxCollider2D spawnArea; // 몬스터
    [SerializeField] private Tilemap floorTilemap; // 장애물

    [Header("장애물 설정")]
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private int minObstacle;
    [SerializeField] private int maxObstacle;
    [SerializeField][Tooltip("장애물을 배치할 가상 그리드의 한 칸(Cell)의 크기")] private float cellSize;

    [Header("적 설정")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private int minEnemy;
    [SerializeField] private int maxEnemy;

    [Header("계층")]
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private Transform enemyParent;

    // 방이 생성되면서 생긴 장애물의 위치를 기억하는 변수
    private List<Bounds> spawnedObstacleBounds = new List<Bounds>();

    // 컴포넌트
    private Room room;

    private void Awake()
    {
        room = GetComponent<Room>();
    }

    // RoomManager에서 호출하는 방 생성 메서드
    public void GenerateRoom()
    {
        //DrawAllTileBounds(); // 타일 디버그용
        Debug.Log("<color=orange> RoomGenerator: 생성 명령을 받았습니다. 이제 장애물과 몬스터를 생성합니다.</color>");
        // 이전 방 정보 초기화
        spawnedObstacleBounds.Clear();

        // 장애물, 몬스터 생성
        SpawnObstacles();
        SpawnEnemys();
    }

    // 장애물 생성 메서드
    // 동작 방식
    // SpawnArea를 CellSize 크기의 격자로 나눈다.
    // 모든 격자의 중심 위치를 리스트에 추가
    // 리스트의 순서를 SuffleList로 섞음
    // 섞인 리스트의 맨 앞에서부터 정해진 개수만큼의 위치에 장애물을 생성
    private void SpawnObstacles()
    {
        // 장애물 생성이 가능한 위치 목록
        List<Vector2> possiblePositions = new List<Vector2>();
        //Bounds areaBounds = spawnArea.bounds;
        Bounds areaBounds = floorTilemap.localBounds;

        floorTilemap.CompressBounds(); // 불필요한 빈 공간을 제거해 범위를 압축
        BoundsInt tileBounds = floorTilemap.cellBounds;

        // 모든 타일 좌표를 순회
        for (int x = tileBounds.xMin; x < tileBounds.xMax; x++)
        {
            for (int y = tileBounds.yMin; y < tileBounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // 해당 타일 좌표에 실제로 타일이 있는지 확인 (가장자리 등 빈 공간 제외)
                if (floorTilemap.HasTile(tilePosition))
                {
                    // 타일 좌표를 월드 좌표(타일의 정중앙)로 변환하여 리스트에 추가
                    possiblePositions.Add(floorTilemap.GetCellCenterWorld(tilePosition));
                }
            }
        }

        Debug.Log($"<color=lime>장애물 생성 가능 위치 {possiblePositions.Count}개를 찾았습니다.</color>");
        // 위치 목록을 무작위로 섞기
        ShuffleList(possiblePositions);

        // 섞인 목록의 앞에서부터 정해진 개수만큼 장애물 생성
        int obstacleCount = Random.Range(minObstacle, maxObstacle + 1);
        // 생성하려는 장애물 수가 가능한 위치의 수보다 많지 않도록 보정
        obstacleCount = Mathf.Min(obstacleCount, possiblePositions.Count);

        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
            Vector2 spawnPosition = possiblePositions[i]; // 섞인 리스트에서 순서대로 위치를 꺼냄

            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, obstacleParent);
        }

    }

    void DrawAllTileBounds()
    {
        if (floorTilemap == null) return;

        Grid grid = floorTilemap.layoutGrid;
        BoundsInt tileBounds = floorTilemap.cellBounds;

        for (int x = tileBounds.xMin; x < tileBounds.xMax; x++)
        {
            for (int y = tileBounds.yMin; y < tileBounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (floorTilemap.HasTile(tilePos))
                {
                    // 타일의 네 모서리 월드 좌표를 가져옵니다.
                    Vector3 p1 = grid.CellToWorld(new Vector3Int(x, y, 0));
                    Vector3 p2 = grid.CellToWorld(new Vector3Int(x + 1, y, 0));
                    Vector3 p3 = grid.CellToWorld(new Vector3Int(x, y + 1, 0));
                    Vector3 p4 = grid.CellToWorld(new Vector3Int(x + 1, y + 1, 0));

                    // 네 모서리를 이어 보라색 사각형을 그립니다.
                    Debug.DrawLine(p1, p2, Color.magenta, 15f);
                    Debug.DrawLine(p1, p3, Color.magenta, 15f);
                    Debug.DrawLine(p2, p4, Color.magenta, 15f);
                    Debug.DrawLine(p3, p4, Color.magenta, 15f);
                }
            }
        }
    }

    // 리스트의 원소를 무작위로 섞는다.
    // 음악 재생리스트에서 랜덤 순서 재생과 비슷한 방식이다.
    private void ShuffleList(List<Vector2> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // 0부터 i까지의 인덱스 중 하나를 무작위로 선택
            int randomIndex = Random.Range(0, i + 1);

            // 선택된 인덱스의 값과 현재 인덱스(i)의 값을 교환
            Vector2 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
        


    // 몬스터 생성 메서드
    private void SpawnEnemys()
    {
        Debug.Log("<color=cyan> SpawnEnemys 메서드 시작! </color>"); // 디버그 용도
        Room room = GetComponent<Room>();
        Transform playerTransform = GameManager.Instance.Player.transform;

        int enemyCount = Random.Range(minEnemy, maxEnemy + 1);
        Debug.Log($"<color=yellow>생성 시도할 적의 수: {enemyCount}</color>"); // 디버그 용도

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            // 유효한 위치를 찾을 때까지 최대 100번 반복
            for(int attempt = 0; attempt < 100; attempt++)
            {
                Vector2 randomPosition = GetRandomPointInSpawnArea();

                if(IsPositionValid(randomPosition, 1.0f)) // 검사하는 범위 (Cell과 크기가 동일하여 한 Cell에 최대 한 마리의 몬스터만 생성)
                {
                    // 적 프리팹을, 랜덤한 위치에, 회전 없이, enemyParent의 하위 오브젝트로 생성한다.
                    GameObject enemyInstance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity, enemyParent);

                    EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();

                    if (enemyController != null)
                    {
                        // 수정 전
                        //room.RegisterEnemy(enemyScript); // Room에 Enemy를 등록 (Enemy연동 전)

                        // 수정 후
                        // 주원님 enemyController에서 Init을 받아옴
                        enemyController.Init(playerTransform, room);
                        // 그리고 생성한 몬스터 목록을 방에 등록한다.
                        room.RegisterEnemy(enemyController);
                    }

                    break;
                }
            }
        }
    }

    // 스폰 영역 내에서 랜덤한 좌표를 반환하는 헬퍼 함수
    private Vector2 GetRandomPointInSpawnArea()
    {
        //Bounds bounds = spawnArea.bounds;
        Bounds bounds = floorTilemap.localBounds;
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }

    // 해당 위치에 장애물이 있는지 확인하여 값을 리턴하는 메서드
    // !! 중요 !! 모든 장애물 프리팹은 꼭 Layer를 Obstacle로 잘 선택해놓아야 합니다.
    private bool IsPositionValid(Vector2 position, float checkRadius)
    {
        // 지정한 위치와 반경 내에 'Obstacle' 레이어를 가진 콜라이더가 있는지 확인
        // LayerMask를 사용하면 특정 레이어만 검사할 수 있다.
        // 주소를 올려도 되는지 모르겠는데 https://m.blog.naver.com/pxkey/221324857701 에서 OverlapCircle 정보를 찾았습니다.
        int obstacleLayer = LayerMask.GetMask("Obstacle");
        Collider2D blockingCollider = Physics2D.OverlapCircle(position, checkRadius, obstacleLayer);

        // blockingCollider이 null 이면 해당 위치에 장애물이 없다는 뜻
        return blockingCollider == null;
    }
}
