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
        // 벽 충돌
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile();
        }
        // 타겟과 충돌
        else if (rangeWeaponHandler.Target.value == (rangeWeaponHandler.Target.value | (1 << collision.gameObject.layer)))
        {
            // 데미지 계산
            ResourceController targetResource = collision.GetComponent<ResourceController>();
            if (targetResource != null)
            {
                targetResource.ChangeHealth(-rangeWeaponHandler.Power);
                if (rangeWeaponHandler.IsOnKnockBack)
                {
                    // 넉백 처리
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

        // 오브젝트의 오른쪽을 _direction 방향으로 바라보게 회전
        transform.right = this.direction;

        if (direction.x < 0)
        {
            // 피벗을 회전 시켜줘야 투사체가 제대로 보인다(?)
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
