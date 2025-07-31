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
    private AnimationHandler animationHandler;
    public AnimationHandler AnimationHandler { get { return animationHandler; } }
    private StatHandler statHandler;
    public StatHandler StatHandler { get { return statHandler; } }

    [Header("공격 패턴")]
    public List<BossAttackSO> attackPatterns; // 보스가 사용할 스킬 목록

    [Header("타겟 정보")]
    [SerializeField] private Transform target; // 플레이어의 Transform
    public Transform Target { get { return target; } }
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

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody2D component is missing on " + gameObject.name);
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

    private void OnDead()
    {
        Debug.Log("Boss defeated!");
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
}
