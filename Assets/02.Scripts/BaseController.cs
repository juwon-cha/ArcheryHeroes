using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rigid;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] public WeaponHandler weaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    protected Vector2 moveDirection = Vector2.zero;
    public Vector2 MoveDirecition { get => moveDirection; }

    protected Vector2 lookDirection;
    public Vector2 LookDirtection { get => lookDirection; }

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (weaponPrefab != null)
        {
            weaponHandler = Instantiate(weaponPrefab, weaponPivot);
        }
        else
        {
            weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        lookDirection = moveDirection;
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();
    }
    protected virtual void FixedUpdate()
    {
        Movement(moveDirection);
    }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction)
    {
        direction = direction * 5;
        rigid.velocity = direction;
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90 ? true : false;

        spriteRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        weaponHandler?.Rotate(isLeft);
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        if(timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if(isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (MoveDirecition == Vector2.zero)
        {
            weaponHandler?.Attack();
        }
    }
}
