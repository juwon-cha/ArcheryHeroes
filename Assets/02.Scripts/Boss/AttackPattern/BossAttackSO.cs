using UnityEngine;

public abstract class BossAttackSO : ScriptableObject
{
    [Header("공격 기본 정보")]
    public string AttackName;
    public float Cooldown = 2f; // 이 공격을 사용한 후의 쿨타임

    public abstract void Execute(BossController boss);
}
