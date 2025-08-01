using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{
    // 상태 진입
    public abstract void EnterState(BossController bossController);

    // 상태 업데이트
    public abstract void UpdateState(BossController bossController);

    // 상태 종료
    public abstract void ExitState(BossController bossController);
}
