using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossState
{
    public override void EnterState(BossController boss)
    {
        Debug.Log("보스: 추적 시작!");

        // 첫 번째 공격 패턴의 공격 범위를 기준으로 설정 (혹은 보스 자체의 attackRange 변수 사용)
        if (boss.attackPatterns.Count > 0)
        {
            // 예시: 첫 번째 공격 패턴의 쿨타임 등을 기반으로 공격 범위 설정 (SO에 attackRange 변수 추가 필요)
            // attackRange = boss.attackPatterns[0].attackRange; 
            //attackRange = 7f; // 임시 값
        }
    }

    public override void UpdateState(BossController boss)
    {
        if (boss.Target == null)
        {
            // 타겟이 없으면 대기 상태로 돌아감
            boss.ChangeState(new BossIdleState(1f));
            return;
        }

        float distance = boss.DistanceToTarget();

        // 추적 포기 범위 확인
        if (distance > boss.ChaseRange)
        {
            Debug.Log("보스: 플레이어가 너무 멀어져서 추적 포기.");
            boss.ChangeState(new BossIdleState(1f));
            return;
        }

        // 공격 범위 확인
        if (distance <= boss.AttackRange)
        {
            Debug.Log("보스: 플레이어가 공격 범위에 들어옴!");
            // 공격 패턴을 랜덤으로 골라 공격 상태로 전환
            int index = Random.Range(0, boss.attackPatterns.Count);
            boss.ChangeState(new BossAttackState(boss.attackPatterns[index]));
            return;
        }

        // 위 조건에 모두 해당하지 않으면, 플레이어를 향해 이동
        Vector2 direction = boss.DirectionToTarget();
        boss.LookDirection = direction;

        // 보스의 스탯에 있는 속도(Speed)를 사용
        boss.Rigidbody.velocity = direction * boss.StatHandler.Speed;

        // 추적 애니메이션 재생
        boss.AnimationHandler.Move(boss.Rigidbody.velocity);
    }

    public override void ExitState(BossController boss)
    {
        boss.StopMovement(); // 상태가 바뀔 때 움직임을 멈추는 것이 안전함

        // TODO: 추적 애니메이션 종료
        // boss.animator.SetBool("IsChasing", false);
    }
}
