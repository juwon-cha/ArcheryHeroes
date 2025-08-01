using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    [SerializeField] private bool showGizmos = true; // 인스펙터에서 기즈모 표시 여부를 제어
    private Transform _target;

    [SerializeField] private float followRange = 15f;
    DungeonRoom parentRoom; // 몬스터가 속한 방

    public void Init(Transform target, DungeonRoom room)
    {
        _target = target;
        parentRoom = room;
    }

    protected override void Attack()
    {
        if (lookDirection != Vector2.zero)
        {
            weaponHandler?.Attack();
        }
    }

    public override void OnDead()
    {
        parentRoom.OnEnemyKill(this);
        base.OnDead();
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (_target.position - transform.position).normalized; // 방향 계산
    }

    protected override void HandleAction()
    {
        if (weaponHandler == null || _target == null)
        {
            if (!movementDirection.Equals(Vector2.zero))
            {
                movementDirection = Vector2.zero; // 타겟이 없거나 무기가 없으면 이동하지 않음
                return;
            }
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        // 플레이어를 따라갈 범위 안에 들어왔다면
        if (distance <= followRange)
        {
            // 플레이어를 바라봄
            lookDirection = direction;

            // 공격 범위 안에 들어왔다면
            if (distance < weaponHandler.AttackRange)
            {
                // 레이캐스트로 충돌 검사
                // 레이어마스크를 통해 충돌 구분
                int layerMaskTarget = weaponHandler.Target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position,
                    direction, weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero;
                return;
            }

            movementDirection = direction;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmos를 사용하여 followRange 시각화
        if(!showGizmos)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
        // 공격 범위 시각화
        if (weaponHandler != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, weaponHandler.AttackRange);
        }
    }
}
