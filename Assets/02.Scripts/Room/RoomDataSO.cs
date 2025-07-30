using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Room")]
public class RoomDataSO : ScriptableObject
{
    // 정적 데이터의 영역, 설계도 같은 개념
    // 방 정보
    public int width;
    public int height;
    public List<GameObject> obstaclePrefabs;
    public int minObstacle;
    public int maxObstacle;

    // 몬스터 정보
    public List<GameObject> enemyPrefabs; // 일단 임시 GameObject로 선언
    public int count; // 몬스터 등장 마리수
    public RoomType roomType;
}

public enum RoomType
{
    normal,
    boss,
    shop
}