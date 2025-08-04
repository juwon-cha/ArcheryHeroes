using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDungeonDoor : DungeonDoor
{
    public TutorialDungeonManager manager;
    public TutorialDungeonRoom tutorialDungeonRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 문이 열려있는 상태이고, 플레이어일 때
        Debug.Log("플레이어가 문 트리거에 닿음");
        if (collision.CompareTag("Player"))
        {
            if (tutorialDungeonRoom.spawnedMonsters.Count > 0)
            {
                for (int i = tutorialDungeonRoom.spawnedMonsters.Count - 1; i >= 0; i--)
                {
                    ObjectPoolingManager.Instance.Return(tutorialDungeonRoom.spawnedMonsters[i]);
                }
                tutorialDungeonRoom.tutorialUI.next.SetActive(false);
                tutorialDungeonRoom.spawnedMonsters.Clear();
            }
            if (manager.currentStageIndex == 2)
                SceneManager.LoadScene("MainScene");
            Debug.Log("플레이어가 다음 스테이지로 이동합니다.");
            // 던전 매니저에게 다음 방을 생성해 달라고 요청
            TutorialDungeonManager.Instance.LoadNextRoom();
        }
    }
}
