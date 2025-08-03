using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    [Header("Monster Data")]
    [SerializeField] private EnemyDataSO enemyData;

    [SerializeField] private bool showGizmos = true; // 인스펙터에서 기즈모 표시 여부를 제어
    private Transform _target;

    //[SerializeField] private float followRange = 15f;
    DungeonRoom parentRoom; // 몬스터가 속한 방
    ResourceController resourceController;

    private void Awake()
    {
        if(enemyData != null)
        {
            ApplyEnemyData();
        }

        base.Awake();

        resourceController = GetComponent<ResourceController>();
    }

    private void ApplyEnemyData()
    {
        weaponPrefab = enemyData.weaponPrefab;

        if (statHandler != null)
        {
            statHandler.Initialize(enemyData);
        }
    }

    public void Init(Transform target, DungeonRoom room)
    {
        _target = target;
        parentRoom = room;

        if (resourceController != null && enemyData != null)
        {
            resourceController.RestoreAndReset(); // 체력을 최대로 회복
        }
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

        // TODO: 경험치 지급
        if(enemyData != null)
        {
            Debug.Log($"{enemyData.enemyName} 처치! 경험치 {enemyData.xpValue} 획득!");

            AchievementManager.Instance.UpdateKillEnemyByTypeProgress(enemyData.enemyType, 1);
            AchievementManager.Instance.UpdateProgress(MissionType.KillEnemy, 1);

            GameManager.Instance.GainExp(enemyData.xpValue);
        }

        base.OnDead();
        parentRoom.OnEnemyKill(this);
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
            movementDirection = Vector2.zero; // 타겟이 없거나 무기가 없으면 이동하지 않음
            return;
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        // 플레이어를 따라갈 범위 안에 들어왔다면
        if (distance <= enemyData.followRange)
        {
            // 플레이어를 바라봄
            lookDirection = direction;

            // 공격 범위 안에 들어왔다면
            if (distance < enemyData.attackRange)
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
        Gizmos.DrawWireSphere(transform.position, enemyData.followRange);
        // 공격 범위 시각화
        if (weaponHandler != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
        }
    }

    private void OnEnable()
    {
        base.OnRestore(); // base에서 모든 부활 기능을 사용

        if (resourceController != null)
        {
            resourceController.RestoreAndReset(); // 체력 등을 초기화
        }

        isAttacking = false;
        movementDirection = Vector2.zero;
    }
}
