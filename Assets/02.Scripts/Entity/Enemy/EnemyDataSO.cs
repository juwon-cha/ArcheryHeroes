using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public string enemyName = "몬스터"; // 몬스터 이름
    public int xpValue = 10;           // 지급 경험치
    public EnemyType enemyType = EnemyType.None; // 몬스터 타입

    [Header("전투 스탯")]
    public float maxHealth = 100f;     // 최대 체력
    public float speed = 3f;           // 이동 속도

    [Header("AI 및 행동")]
    public float followRange = 15f;    // 플레이어 추적 시작 범위
    public float attackRange = 1.5f;   // 공격 범위

    [Header("장비")]
    public WeaponHandler weaponPrefab; // 사용할 무기 프리팹
}
