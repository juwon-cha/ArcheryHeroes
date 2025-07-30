using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    [Header("방 프리팹 목록")]
    [Tooltip("일반 방으로 사용될 '방 프리팹' 목록")]
    [SerializeField] private List<GameObject> normalRoomPrefabs;

    [Tooltip("보스 방으로 사용될 '방 프리팹'")]
    [SerializeField] private GameObject bossRoomPrefab;

    private GameObject currentRoomInstance;
    private int currentStage = 0;
    private Room currentRoom;

    private void Start()
    {
        Debug.Log("<color=red>1. RoomManager가 시작되었습니다!</color>"); // 디버그용
        SpawnNextRoom();
    }

    public void SpawnNextRoom()
    {
        if (currentRoomInstance != null)
        {
            Destroy(currentRoomInstance);
        }

        currentStage++;

        // 사용할 방 프리팹 선택 (예: 3 스테이지마다 보스방)
        GameObject selectedRoomPrefab;
        if (currentStage % 3 == 0 && bossRoomPrefab != null)
        {
            selectedRoomPrefab = bossRoomPrefab;
        }
        else
        {
            selectedRoomPrefab = normalRoomPrefabs[Random.Range(0, normalRoomPrefabs.Count)];
        }

        Debug.Log("<color=orange>2. 다음 방 생성을 시도합니다. 사용할 프리팹: " + selectedRoomPrefab.name + "</color>"); // 디버그용
        // 선택된 '방 프리팹'을 씬에 생성
        currentRoomInstance = Instantiate(selectedRoomPrefab, Vector3.zero, Quaternion.identity);
        Debug.Log("<color=green>3. 방 프리팹 인스턴스화 성공!</color>"); // 디버그용

        RoomGenerator generator = currentRoomInstance.GetComponent<RoomGenerator>();
        if (generator == null)
        {
            Debug.LogError("치명적 오류: 생성된 방에 RoomGenerator 스크립트가 없습니다!"); // 에러 로그 추가!
        }
        else
            generator.GenerateRoom();

        // RoomGenerator는 프리팹에 이미 붙어있으므로, 그 안의 로직이 실행되도록 함
        // 만약 RoomGenerator의 GenerateRoom()이 public이라면 아래처럼 명시적으로 호출 가능
        // currentRoomInstance.GetComponent<RoomGenerator>().GenerateRoom();
        // 만약 Start()나 Awake()에 로직이 있다면 자동으로 실행됨
    }
    public void OnRoomCleared()
    {
        Debug.Log($"스테이지 {currentStage} 클리어! 다음 방으로 이동합니다.");
        // 문을 열거나, 포탈을 활성화하는 로직을 추가할 거면 이곳에서

        // 임시로 2초 후에 다음 방 생성
        Invoke(nameof(SpawnNextRoom), 2f);
    }
}

