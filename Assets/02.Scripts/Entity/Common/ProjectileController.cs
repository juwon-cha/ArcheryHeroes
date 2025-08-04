using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;
    private int bounceCount = 0; // 충돌 시 튕길 횟수

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
            if (bounceCount <= 0)
            {
                DestroyProjectile();
            }
            else
            {
                // 방향의 정반대 방향으로 raycast 쏴서 법선 추정
                Vector2 rayOrigin = (Vector2)transform.position + direction * 0.1f;
                RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, direction, 0.5f, levelCollisionLayer);

                foreach (var hit in hits)
                {
                    if (hit.collider.gameObject == gameObject) continue; // 자기 자신 무시

                    // 첫 번째 유효한 충돌 처리
                    Vector2 normal = hit.normal;
                    direction = Vector2.Reflect(direction.normalized, normal).normalized;

                    Init(direction, rangeWeaponHandler);
                    transform.position += (Vector3)normal * 0.2f;
                    bounceCount--;
                    break;
                }

            }
        }
        // 타겟과 충돌
        else if (rangeWeaponHandler.Target.value == (rangeWeaponHandler.Target.value | (1 << collision.gameObject.layer)))
        {
            // 데미지 계산
            ResourceController targetResource = collision.GetComponent<ResourceController>();
            if (targetResource != null)
            {
                targetResource.ChangeHealth(-rangeWeaponHandler.Power, rangeWeaponHandler.ElementType);
                if (rangeWeaponHandler.IsOnKnockBack)
                {
                    // 넉백 처리
                    BaseController controller = collision.GetComponent<BaseController>();
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

        bounceCount = rangeWeaponHandler.bounceCount; // 튕길 횟수 초기화
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
