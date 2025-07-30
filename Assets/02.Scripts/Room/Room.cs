using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private List<EnemyController> activeEnemies = new List<EnemyController>();

    // RoomGenerator가 적을 생성할 때 이 함수를 호출해줘야 함
    public void RegisterEnemy(EnemyController enemy)
    {
        activeEnemies.Add(enemy);
    }

    // Enemy가 죽을 때 스스로 이 함수를 호출해야 함
    public void OnEnemyDied(EnemyController enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        // 모든 적이 제거되었는지 확인
        if (activeEnemies.Count == 0)
        {
            RoomCleared();
        }
    }

    private void RoomCleared()
    {
        // 방이 클리어되었음을 던전 매니저에게 알림
        RoomManager.Instance.OnRoomCleared();
    }
}
