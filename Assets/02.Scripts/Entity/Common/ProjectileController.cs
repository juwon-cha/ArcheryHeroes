using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedWeaponHandler rangeWeaponHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D rigibody;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigibody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration += Time.deltaTime;

        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile();
        }

        rigibody.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �� �浹
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile();
        }
        // Ÿ�ٰ� �浹
        else if (rangeWeaponHandler.Target.value == (rangeWeaponHandler.Target.value | (1 << collision.gameObject.layer)))
        {
            // ������ ���
            ResourceController targetResource = collision.GetComponent<ResourceController>();
            if (targetResource != null)
            {
                targetResource.ChangeHealth(-rangeWeaponHandler.Power);
                if (rangeWeaponHandler.IsOnKnockBack)
                {
                    // �˹� ó��
                    EnemyController controller = collision.GetComponent<EnemyController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockBack(transform, rangeWeaponHandler.KnockBackPower, rangeWeaponHandler.KnockBackDuration);
                    }
                }
            }

            DestroyProjectile();
        }
    }

    public void Init(Vector2 direction, RangedWeaponHandler weaponHandler)
    {
        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0f;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        // ������Ʈ�� �������� _direction �������� �ٶ󺸰� ȸ��
        transform.right = this.direction;

        if (direction.x < 0)
        {
            // �ǹ��� ȸ�� ������� ����ü�� ����� ���δ�(?)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            pivot.localRotation = Quaternion.Euler(0, 0, 0);
        }

        isReady = true;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
