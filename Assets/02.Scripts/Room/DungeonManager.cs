using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DungeonManager : Singleton<DungeonManager>
{
    private Action<int> OnStageChanged; // 스테이지 변경 이벤트

    [Header("방 프리팹 목록")]
    [SerializeField] private List<GameObject> roomPrefabs;
    [SerializeField] private GameObject bossRoomPrefab;
    [SerializeField] private GameObject eventRoomPrefab;
    [SerializeField] private int clearStageIndex = 11; // 게임 클리어 스테이지 인덱스 (예: 11 스테이지에서 게임 클리어)

    private GameObject currentRoomInstance; // 현재 방의 정보
    private int currentStageIndex = 0; // 현재 스테이지 정보
    private GameObject player; // 플레이어 정보
    private Transform playerTransform; // 플레이어의 위치

    public int CurrentStageIndex { get => currentStageIndex; }

    protected override void Initialize()
    {
        currentStageIndex = 0;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ResetDungeon()
    {
        currentStageIndex = 0;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene" || scene.name == "MainScene" || scene.name == "TutorialScene" || scene.name == "CustomizingScene") return;

        player = GameManager.Instance.Player;

        if (player != null)
            playerTransform = GameManager.Instance.Player.transform;
        else
            Debug.Log("GameManager에서 Player를 찾을 수 없습니다.");

        LoadNextRoom();
    }

    // 다음 방을 불러오는 메서드
    public void LoadNextRoom()
    {
        currentStageIndex++;
        Debug.Log($"현재 스테이지는 {currentStageIndex} 스테이지");

        // 현재 스테이지가 마지막 스테이지라면 게임 클리어
        if(currentStageIndex >= clearStageIndex)
        {
            Debug.Log("게임 클리어!");
            UIManager.Instance.ShowUI(UIType.GameClear);
            return;
        }

        // 다음 방을 로드하기 전에 남아있는 보스 공격 패턴들을 모두 제거
        ClearRemainingBossAttackPatterns();

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
                Debug.Log("보스 방을 불러옵니다.");
                roomToLoad = bossRoomPrefab;
            }
        }

        // 이번 스테이지가 5단위 스테이지라면 (2순위)
        else if (currentStageIndex % 5 == 0)
        {
            if(eventRoomPrefab != null)
            {
                Debug.Log("이벤트 방을 불러옵니다.");
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

        // 스테이지 변경 이벤트를 호출한다.
        OnStageChanged?.Invoke(currentStageIndex);
    }

    // 현재 스테이지 인덱스를 반환하는 메서드
    public void AddStageChangedEvent(Action<int> action)
    {
        OnStageChanged += action;
    }

    public void RemoveStageChangedEvent(Action<int> action)
    {
        OnStageChanged -= action;
    }

    private void ClearRemainingBossAttackPatterns()
    {
        // BossAttackObject, BossAttackProjectile 태그를 가진 모든 게임 오브젝트를 배열로 찾아옴
        GameObject[] remainingObject = GameObject.FindGameObjectsWithTag("BossAttackObject");
        GameObject[] remainingProjectiles = GameObject.FindGameObjectsWithTag("BossAttackProjectile");

        // 탄막이 아닌 패턴들은 파괴
        foreach (GameObject go in remainingObject)
        {
            Destroy(go);
        }

        // 탄막 패턴은 오브젝트 풀에 반환
        foreach(GameObject projectile in remainingProjectiles)
        {
            projectile.SetActive(false);
        }
    }
}
