using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer; // 벽 레이어

    // 투사체가 직접 소유하게 될 데이터
    private float speed;
    private float duration;
    private float power;
    private LayerMask targetLayer;
    private bool applyKnockback;
    private float knockbackPower;
    private float knockbackDuration;

    private float currentLifeTime; // 현재까지 살아있었던 시간
    private Vector2 direction;
    private bool isReady;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isReady)
        {
            return;
        }

        // 투사체를 설정된 속도로 계속 이동시킴
        rigidbody.velocity = direction * speed;
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        // 수명 체크
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > duration)
        {
            DestroyProjectile();
        }
    }

    public void Init(Vector2 dir, float spd, float dur, float damage, LayerMask target, bool isKnockback, float knockPower, float knockDuration)
    {
        // 필요한 모든 데이터를 외부(SO)로부터 전달받아 저장
        direction = dir.normalized;
        speed = spd;
        duration = dur;
        power = damage;
        targetLayer = target;
        applyKnockback = isKnockback;
        knockbackPower = knockPower;
        knockbackDuration = knockDuration;

        currentLifeTime = 0f;

        // 스프라이트의 오른쪽(transform.right)이 발사 방향을 향하도록 회전
        transform.right = direction;

        isReady = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 비활성화 상태이거나, 이미 파괴 과정에 들어갔으면 무시
        if (!isReady)
        {
            return;
        }

        // 벽과 충돌했는지 확인 (비트 연산으로 레이어 마스크 포함 여부 체크)
        if ((levelCollisionLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            DestroyProjectile();
            return; // 벽과 충돌 시 아래 로직은 실행하지 않음
        }

        // 타겟과 충돌했는지 확인
        if ((targetLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("Hit Player!");

            // TODO: 플레이어의 체력을 관리하는 컴포넌트로 수정 필요
            ResourceController targetResource = collision.GetComponent<ResourceController>();
            if (targetResource != null)
            {
                targetResource.ChangeHealth(-power);
            }

            // 넉백 처리
            if (applyKnockback)
            {
                // TODO: 플레이어의 컨트롤러로 수정 필요
                BaseController controller = collision.GetComponent<BaseController>();
                if (controller != null)
                {
                    controller.ApplyKnockBack(transform, knockbackPower, knockbackDuration);
                }
            }

            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        isReady = false;

        // TODO: 파티클 이펙트나 사운드를 재생

        //Destroy(gameObject);
        gameObject.SetActive(false); // 오브젝트 풀링을 사용
    }
}
