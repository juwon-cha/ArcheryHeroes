using UnityEngine;

public class BossAttackState : BossState
{
    private BossAttackSO currentAttack;
    private float attackCooldown;

    public BossAttackState(BossAttackSO attack)
    {
        currentAttack = attack;
        //attackCooldown = attack.cooldown;
    }

    public override void EnterState(BossController bossController)
    {
        Debug.Log("Entering Attack State");

        // 공격 상태에서는 이동을 멈춤
        bossController.StopMovement();

        // 스크립터블 오브젝트에게 공격 실행 요청
        currentAttack.Execute(bossController);        
    }

    public override void UpdateState(BossController bossController) { }
    public override void ExitState(BossController bossController) { }
}
