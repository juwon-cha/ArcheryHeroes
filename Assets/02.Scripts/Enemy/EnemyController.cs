using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // BaseController
    protected Rigidbody2D rigidBody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    // 이동 방향
    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    // 바라보는 방향
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockBack = Vector2.zero;
    private float knockBackDuration = 0.0f;

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    [SerializeField] private WeaponHandler weaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    // EnemyController
    [SerializeField] private bool showGizmos = true; // 인스펙터에서 기즈모 표시 여부를 제어
    private Transform _target;

    [SerializeField] private float followRange = 15f;

    protected virtual void Awake()
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

        if (weaponPrefab != null)
        {
            weaponHandler = Instantiate(weaponPrefab, weaponPivot);
            if (weaponHandler == null)
            {
                Debug.LogError("WeaponHandler component is missing on " + gameObject.name);
            }
        }
        else
        {
            // 이미 WeaponHandler가 자식으로 존재하는 경우
            weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
    }

    private void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();
    }

    private void FixedUpdate()
    {
        Movement(movementDirection);
        if (knockBackDuration > 0.0f) // 넉백 지속시간이 남아있다면
        {
            knockBackDuration -= Time.fixedDeltaTime; // 넉백 지속시간 감소
        }
    }

    public void Init(Transform target)
    {
        _target = target;
    }

    private void Movement(Vector2 direction)
    {
        direction *= statHandler.Speed;
        if (knockBackDuration > 0.0f) // 넉백 지속시간이 남아있다면
        {
            direction *= 0.2f; // 기존 이동방향의 힘을 줄여줌
            direction += knockBack; // 넉백의 힘만 넣어주겠다.
        }

        rigidBody.velocity = direction; // Rigidbody2D의 속도에 적용
        animationHandler.Move(direction); // 애니메이션 핸들러에 이동 방향 전달
    }

    private void Rotate(Vector2 direction)
    {
        // y값과 x값을 받아서 그 사이의 세타값을 구한다. 라디안 값이 나옴 -> 라디안을 각도로 바꿈
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool bIsLeft = Mathf.Abs(rotZ) > 90f; // 90도 보다 크면 왼쪽을 바라보고 있다고 판단

        characterRenderer.flipX = bIsLeft; // 왼쪽을 바라보면 flipX를 true로 설정

        if (weaponPivot != null)
        {
            // 무기 피봇을 바라보는 방향으로 회전
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        weaponHandler?.Rotate(bIsLeft); // 무기 핸들러에 회전 방향 전달
    }

    public void ApplyKnockBack(Transform other, float power, float duration)
    {
        knockBackDuration = duration; // 넉백 지속시간 설정
        knockBack = -(other.position - transform.position).normalized * power; // 넉백 방향 설정
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
        {
            return; // 무기 핸들러가 없으면 공격 딜레이를 처리하지 않음
        }

        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime; // 공격 딜레이 시간 증가
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0f; // 공격 딜레이 시간 초기화
            Attack(); // 공격 딜레이가 끝나면 공격 실행
        }
    }

    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
        {
            weaponHandler?.Attack();
        }
    }

    public virtual void OnDead()
    {
        // BaseController
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

        Destroy(gameObject, 2f); // 2초 후에 오브젝트 삭제

        // EnemyController
        EnemyManager.Instance.RemoveEnemyOnDead(this);
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (_target.position - transform.position).normalized; // 방향 계산
    }

    private void HandleAction()
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
