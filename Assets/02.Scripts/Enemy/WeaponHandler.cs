using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float power = 1f;
    public float Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask Target;

    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockBack = false;
    public bool IsOnKnockBack { get => isOnKnockBack; set => isOnKnockBack = value; }

    [SerializeField] private float knockBackPower = 0.1f;
    public float KnockBackPower { get => knockBackPower; set => knockBackPower = value; }

    [SerializeField] private float knockBackDuration = 0.5f;
    public float KnockBackDuration { get => knockBackDuration; set => knockBackDuration = value; }

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public EnemyController EnemyController { get; private set; }
    private Animator animator;
    private SpriteRenderer weaponRenderer;

    protected virtual void Awake()
    {
        EnemyController = GetComponentInParent<EnemyController>();
        if (EnemyController == null)
        {
            Debug.LogError("EnemyController component is missing on " + gameObject.name);
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }

        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
        if (weaponRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on " + gameObject.name);
        }

        animator.speed = 1.0f / delay; // �ִϸ��̼� �ӵ��� ���� �����̿� ���� ����
        transform.localScale = Vector3.one * weaponSize; // ���� ũ�� ����
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Attack()
    {
        AttackAnimation();


        // TODO: ���� ���� ���
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        if (weaponRenderer != null)
        {
            weaponRenderer.flipY = isLeft; // ������ �ٶ󺸸� flipY�� true�� ����
        }
        else
        {
            Debug.LogWarning("WeaponRenderer is not assigned in " + gameObject.name);
        }
    }
}
