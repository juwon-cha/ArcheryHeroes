using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 보스 컨트롤러는 플레이어와 일반 몬스터와 다른 특수한 행동을 가져서 BaseController를 상속하지 않음
public class BossController : MonoBehaviour
{
    // 핵심 컴포넌트
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    public Rigidbody2D Rigidbody { get { return rigidBody; } }
    private BoxCollider2D boxCollider;
    public BoxCollider2D BoxCollider { get { return boxCollider; } }
    private AnimationHandler animationHandler;
    public AnimationHandler AnimationHandler { get { return animationHandler; } }
    private StatHandler statHandler;
    public StatHandler StatHandler { get { return statHandler; } }

    [Header("공격 정보")]
    [SerializeField] private float power = 3f; // 보스의 공격력
    public float Power { get { return power; } }
    public float AdditionalPower { get; set; } = 0f; 

    [SerializeField] private bool applyKnockback = true; // 넉백 적용 여부
    public bool ApplyKnockback { get { return applyKnockback; } }
    [SerializeField] private float knockbackPower = 5f; // 넉백 힘
    public float KnockbackPower { get { return knockbackPower; } }
    [SerializeField] private float knockbackDuration = 0.5f; // 넉백 지속 시간
    public float KnockbackDuration { get { return knockbackDuration; } }

    [Header("기본 정보")]
    public EnemyType enemyType;

    [Header("공격 패턴")]
    public List<BossAttackSO> attackPatterns; // 보스가 사용할 스킬 목록

    [Header("타겟 정보")]
    [SerializeField] private Transform target; // 플레이어의 Transform
    public Transform Target { get { return target; } }
    [SerializeField] private LayerMask targetLayer;
    public Vector2 LookDirection { get; set; } // 보스가 바라보는 방향

    [Header("AI 범위 설정")]
    [SerializeField] private float detectionRange = 20f; // 플레이어를 처음 인식하는 범위
    public float DetectionRange { get { return detectionRange; } }
    [SerializeField] private float chaseRange = 15f;     // 플레이어 추적 범위
    public float ChaseRange { get { return chaseRange; } }
    [SerializeField] private float attackRange = 7f; // 공격을 시작할 범위
    public float AttackRange { get { return attackRange; } }

    [Header("맵 정보")]
    [SerializeField] private Tilemap groundTilemap; // 장판이 생성될 바닥 타일맵
    public Tilemap GroundTilemap { get { return groundTilemap; } }

    private BossState currentState;
    private DungeonRoom parentRoom;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody2D component is missing on " + gameObject.name);
        }

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D component is missing on " + gameObject.name);
        }

        animationHandler = GetComponentInChildren<AnimationHandler>();
        if (animationHandler == null)
        {
            Debug.LogError("AnimationHandler component is missing on " + gameObject.name);
        }

        statHandler = GetComponent<StatHandler>();
        if (statHandler == null)
        {
            Debug.LogError("StatHandler component is missing on " + gameObject.name);
        }
    }

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        // 시작 상태
        ChangeState(new BossChaseState());
    }

    private void Update()
    {
        if(currentState != null)
        {
            currentState.UpdateState(this);
        }

        if (spriteRenderer != null && LookDirection != Vector2.zero)
        {
            spriteRenderer.flipX = LookDirection.x < 0;
        }
    }

    public void ChangeState(BossState state)
    {
        if(currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = state;
        currentState.EnterState(this);
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    public Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized; // 방향 계산
    }

    public void StopMovement()
    {
        rigidBody.velocity = Vector2.zero;
    }

    public void OnDead()
    {
        AchievementManager.Instance.UpdateKillEnemyByTypeProgress(enemyType, 1);
        AchievementManager.Instance.UpdateProgress(MissionType.KillEnemy, 1);

        rigidBody.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false; // 나머지 컴포넌트 비활성화
        }

        if (parentRoom != null)
        {
            parentRoom.OnBossKilled();
        }

        Destroy(gameObject, 2f); // 2초 후에 오브젝트 삭제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 타겟과 충돌했는지 확인
        if ((targetLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("Hit Player!");

            ResourceController targetResource = collision.GetComponent<ResourceController>();
            if (targetResource != null)
            {
                // 기본 공격력과 추가 공격력을 합산하여 최종 데미지 계산
                float totalDamage = power + AdditionalPower;
                targetResource.ChangeHealth(-totalDamage);
                Debug.Log($"플레이어에게 {totalDamage}의 피해를 입혔습니다! (기본: {power}, 추가: {AdditionalPower})");
            }

            // 넉백 처리
            if (applyKnockback)
            {
                BaseController controller = collision.GetComponent<BaseController>();
                if (controller != null)
                {
                    controller.ApplyKnockBack(transform, knockbackPower, knockbackDuration);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 인식 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 추적 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // 공격 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void Init(DungeonRoom room)
    {
        this.parentRoom = room;
    }
}
