using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // BaseController
    protected Rigidbody2D rigidBody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    // �̵� ����
    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    // �ٶ󺸴� ����
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
    [SerializeField] private bool showGizmos = true; // �ν����Ϳ��� ����� ǥ�� ���θ� ����
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
            // �̹� WeaponHandler�� �ڽ����� �����ϴ� ���
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
        if (knockBackDuration > 0.0f) // �˹� ���ӽð��� �����ִٸ�
        {
            knockBackDuration -= Time.fixedDeltaTime; // �˹� ���ӽð� ����
        }
    }

    public void Init(Transform target)
    {
        _target = target;
    }

    private void Movement(Vector2 direction)
    {
        direction *= statHandler.Speed;
        if (knockBackDuration > 0.0f) // �˹� ���ӽð��� �����ִٸ�
        {
            direction *= 0.2f; // ���� �̵������� ���� �ٿ���
            direction += knockBack; // �˹��� ���� �־��ְڴ�.
        }

        rigidBody.velocity = direction; // Rigidbody2D�� �ӵ��� ����
        animationHandler.Move(direction); // �ִϸ��̼� �ڵ鷯�� �̵� ���� ����
    }

    private void Rotate(Vector2 direction)
    {
        // y���� x���� �޾Ƽ� �� ������ ��Ÿ���� ���Ѵ�. ���� ���� ���� -> ������ ������ �ٲ�
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool bIsLeft = Mathf.Abs(rotZ) > 90f; // 90�� ���� ũ�� ������ �ٶ󺸰� �ִٰ� �Ǵ�

        characterRenderer.flipX = bIsLeft; // ������ �ٶ󺸸� flipX�� true�� ����

        if (weaponPivot != null)
        {
            // ���� �Ǻ��� �ٶ󺸴� �������� ȸ��
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        weaponHandler?.Rotate(bIsLeft); // ���� �ڵ鷯�� ȸ�� ���� ����
    }

    public void ApplyKnockBack(Transform other, float power, float duration)
    {
        knockBackDuration = duration; // �˹� ���ӽð� ����
        knockBack = -(other.position - transform.position).normalized * power; // �˹� ���� ����
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
        {
            return; // ���� �ڵ鷯�� ������ ���� �����̸� ó������ ����
        }

        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime; // ���� ������ �ð� ����
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0f; // ���� ������ �ð� �ʱ�ȭ
            Attack(); // ���� �����̰� ������ ���� ����
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
            component.enabled = false; // ������ ������Ʈ ��Ȱ��ȭ
        }

        Destroy(gameObject, 2f); // 2�� �Ŀ� ������Ʈ ����

        // EnemyController
        EnemyManager.Instance.RemoveEnemyOnDead(this);
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (_target.position - transform.position).normalized; // ���� ���
    }

    private void HandleAction()
    {
        if (weaponHandler == null || _target == null)
        {
            if (!movementDirection.Equals(Vector2.zero))
            {
                movementDirection = Vector2.zero; // Ÿ���� ���ų� ���Ⱑ ������ �̵����� ����
                return;
            }
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        // �÷��̾ ���� ���� �ȿ� ���Դٸ�
        if (distance <= followRange)
        {
            // �÷��̾ �ٶ�
            lookDirection = direction;

            // ���� ���� �ȿ� ���Դٸ�
            if (distance < weaponHandler.AttackRange)
            {
                // ����ĳ��Ʈ�� �浹 �˻�
                // ���̾��ũ�� ���� �浹 ����
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
        // Gizmos�� ����Ͽ� followRange �ð�ȭ
        if(!showGizmos)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
        // ���� ���� �ð�ȭ
        if (weaponHandler != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, weaponHandler.AttackRange);
        }
    }
}
