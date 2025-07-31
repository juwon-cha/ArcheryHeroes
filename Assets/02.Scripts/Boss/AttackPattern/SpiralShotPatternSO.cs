using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpiralShotPattern", menuName = "Boss/Attack Patterns/Spiral Shot")]
public class SpiralShotPatternSO : BossAttackSO
{
    [Header("나선 탄막 설정")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float patternDuration = 5f; // 패턴 지속 시간
    [SerializeField] private float fireRate = 0.1f; // 발사 간격 (초)
    [SerializeField] private float rotationSpeed = 100f; // 초당 회전 각도

    [Header("투사체 상세 설정")]
    [SerializeField] private float projectileSpeed = 10f; // 투사체 속도
    [SerializeField] private float projectileDuration = 5f; // 투사체 수명
    [SerializeField] private float projectilePower = 10f; // 투사체 공격력
    [SerializeField] private LayerMask targetLayer;       // 공격할 대상의 레이어
    [SerializeField] private bool applyKnockback = true;
    [SerializeField] private float knockbackPower = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private float patternTimer; // 패턴 전체 시간
    private float fireTimer; // 다음 발사 시간
    private float currentAngle;

    public override void Execute(BossController boss)
    {
        patternTimer = patternDuration;
        fireTimer = 0f; // 바로 첫 발을 쏠 수 있도록 0으로 설정
        currentAngle = 0f; // 시작 각도를 0으로 초기화

        boss.StartCoroutine(SpiralShotCoroutine(boss));
    }

    private IEnumerator SpiralShotCoroutine(BossController boss)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"{AttackName}: projectilePrefab이 할당되지 않았습니다!");
            // 코루틴이 끝나기 전에 다음 상태로 전환하여 게임이 멈추지 않도록 함
            boss.ChangeState(new BossIdleState(Cooldown));
            yield break; // 코루틴 즉시 종료
        }

        Debug.Log($"{boss.name}이(가) {AttackName} 시전!");

        // 정해진 패턴 지속 시간 동안 fireTimer가 0 이하가 될 때마다 투사체를 한 발 발사
        // 각도가 프레임마다 계속해서 점진적으로 변함
        while (patternTimer > 0)
        {
            // 매 프레임마다 타이머들을 감소시킴
            patternTimer -= Time.deltaTime;
            fireTimer -= Time.deltaTime;
            // 매 프레임마다 currentAngle의 값에 rotationSpeed * Time.deltaTime 만큼의 작은 각도를 계속해서 더함
            currentAngle += rotationSpeed * Time.deltaTime; // 각도도 계속 업데이트

            // 발사할 시간이 되었는지 확인(fireTimer가 0이 되면 발사)
            if (fireTimer <= 0)
            {
                fireTimer = fireRate; // 다음 발사를 위해 타이머 초기화

                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
                Vector2 direction = rotation * Vector2.right;

                // 투사체 생성
                GameObject projectileGO = Object.Instantiate(projectilePrefab, boss.transform.position, Quaternion.identity);
                BossProjectileController projectile = projectileGO.GetComponent<BossProjectileController>();

                if (projectile != null)
                {
                    projectile.Init(direction, projectileSpeed, projectileDuration, projectilePower, targetLayer,
                                    applyKnockback, knockbackPower, knockbackDuration);
                }
            }

            // 다음 프레임까지 실행을 잠시 멈추고 대기
            yield return null;
        }

        // while 루프가 끝나면(패턴 지속 시간이 다 되면) 공격 종료
        Debug.Log("나선형 탄막 패턴 종료.");
        boss.ChangeState(new BossIdleState(Cooldown));
    }
}
