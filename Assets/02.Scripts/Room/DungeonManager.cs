using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    [Header("방 프리팹 목록")]
    [SerializeField] private List<GameObject> roomPrefabs;
    [SerializeField] private GameObject bossRoomPrefab;
    [SerializeField] private GameObject eventRoomPrefab;

    private GameObject currentRoomInstance; // 현재 방의 정보
    private int currentStageIndex = 0; // 현재 스테이지 정보
    private GameObject player; // 플레이어 정보
    private Transform playerTransform; // 플레이어의 위치

    private void Start()
    {
        player = GameManager.Instance.Player;

        if (player != null)
        {
            playerTransform = GameManager.Instance.Player.transform;
        }
        else
        {
            Debug.Log("GameManager에서 Player를 찾을 수 없습니다.");
        }
        LoadNextRoom();
    }

    // 다음 방을 불러오는 메서드
    public void LoadNextRoom()
    {
        currentStageIndex++;
        Debug.Log($"현재 스테이지는 {currentStageIndex} 스테이지");
        // 방의 정보가 남아있다. 즉, 이전 방의 정보가 남아있다면 오브젝트 풀링으로 넣어준다.
        if (currentRoomInstance != null)
        {
            ObjectPoolingManager.Instance.Return(currentRoomInstance);
        }

        // Todo: 여기서 방 현재 스테이지에 따라 보스방 or 특별방 을 가져오는 조건 추가하면 된다.
        // 방 프리팹중 한 개를 랜덤으로 가져온다.
        GameObject roomToLoad = null;

        // 이번 스테이지가 10 단위 스테이지라면 (1순위)
        if (currentStageIndex % 10 == 0)
        {
            if (bossRoomPrefab != null)
            {
                roomToLoad = bossRoomPrefab;
            }
        }

        // 이번 스테이지가 5단위 스테이지라면 (2순위)
        else if (currentStageIndex % 5 == 0)
        {
            if(eventRoomPrefab != null)
            {
                roomToLoad = eventRoomPrefab;
            }
        }

        // 위 규칙에 해당하지 않는다면 일반 방을 호출한다.
        if(roomToLoad == null)
        {
            roomToLoad = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        }


        // 가져온 방 프리팹을 오브젝트 풀링으로 현재 방 정보에 넣어준다.
        currentRoomInstance = ObjectPoolingManager.Instance.Get(roomToLoad, Vector3.zero, Quaternion.identity);

        // 현재 방 정보에서 DungeonRoom 컴포넌트를 가져온다.
        DungeonRoom newDungeonRoom = currentRoomInstance.GetComponent<DungeonRoom>();

        if (newDungeonRoom != null && playerTransform != null)
        {
            // 플레이어의 포지션을 던전 방의 스폰 포인트 위치로 변경한다.
            playerTransform.position = newDungeonRoom.GetPlayerSpawnPoint().position;
        }
    }
}
