using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    private float _idleTime = 1.5f; // 대기 시간
    private float timer = 0f; // 타이머

    public BossIdleState(float idleTime)
    {
        _idleTime = idleTime;
    }

    public override void EnterState(BossController bossController)
    {
        Debug.Log("Entering Idle State");
        timer = _idleTime; // 타이머 초기화
        bossController.StopMovement();
    }

    public override void UpdateState(BossController bossController)
    {
        // 타이머가 아직 남아있으면 대기
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (bossController.Target == null)
        {
            return;
        }

        // 플레이어가 인식 범위 안으로 들어왔는지 확인
        if (bossController.DistanceToTarget() <= bossController.DetectionRange)
        {
            // 발견했다면 Detect 상태로 전환
            bossController.ChangeState(new BossDetectState());
        }
    }

    public override void ExitState(BossController bossController) { }
}
