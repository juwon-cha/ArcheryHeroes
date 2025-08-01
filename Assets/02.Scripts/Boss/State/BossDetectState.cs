using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetectState : BossState
{
    private float detectTime; // 인식 상태를 유지할 시간 (애니메이션 길이 등)

    public BossDetectState(float duration = 0.5f)
    {
        detectTime = duration;
    }

    public override void EnterState(BossController boss)
    {
        Debug.Log("보스: 플레이어 발견!");
        boss.StopMovement(); // 잠시 멈춰서 인식하는 연출

        // TODO: 느낌표 이펙트 생성
        

        // LookDirection을 플레이어 방향으로 즉시 설정
        if (boss.Target != null)
        {
            boss.LookDirection = (boss.Target.position - boss.transform.position).normalized;
        }
    }

    public override void UpdateState(BossController boss)
    {
        detectTime -= Time.deltaTime;
        if (detectTime <= 0)
        {
            // 인식 시간이 끝나면 바로 추적 상태로 전환
            boss.ChangeState(new BossChaseState());
        }
    }

    public override void ExitState(BossController boss) { }
}
