using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRadialShotPattern", menuName = "Boss/Attack Patterns/Radial Shot")]
public class RadialShotPatternSO : BossAttackSO
{
    [Header("원형 탄막 설정")]
    [SerializeField] private GameObject projectilePrefab; // 발사할 투사체 프리팹
    [SerializeField] private int projectileCount = 12;    // 투사체 개수

    [Header("연사 설정")]
    [SerializeField] private int burstCount = 3; // 총 몇 번 발사할 것인가
    [SerializeField] private float delayBetweenBursts = 0.5f; // 발사 사이의 간격

    [Header("투사체 상세 설정")]
    [SerializeField] private float projectileSpeed = 10f; // 투사체 속도
    [SerializeField] private float projectileDuration = 5f; // 투사체 수명
    [SerializeField] private float projectilePower = 10f; // 투사체 공격력
    [SerializeField] private LayerMask targetLayer;       // 공격할 대상의 레이어
    [SerializeField] private bool applyKnockback = true;
    [SerializeField] private float knockbackPower = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    public override void Execute(BossController boss)
    {
        boss.StartCoroutine(RadialShotCoroutine(boss));
    }

    private IEnumerator RadialShotCoroutine(BossController boss)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"{AttackName}: projectilePrefab이 할당되지 않았습니다!");
            // 코루틴이 끝나기 전에 다음 상태로 전환하여 게임이 멈추지 않도록 함
            boss.ChangeState(new BossIdleState(Cooldown));
            yield break; // 코루틴 즉시 종료
        }

        Debug.Log($"{boss.name}이(가) {AttackName} 시전! (총 {burstCount}회)");

        // burstCount 만큼 반복
        for (int i = 0; i < burstCount; i++)
        {
            // 기준이 될 시작 각도 무작위 설정
            float randomStartAngle = Random.Range(0f, 360f);

            // 각 투사체 사이의 각도 계산(원형으로 균등하게 배치하기 위함)
            float angleStep = 360f / projectileCount;

            // 한 번에 발사할 투사체를 원형으로 배치
            for (int j = 0; j < projectileCount; j++)
            {
                // 기존 투사체 사이의 각도에 무작위 시작 각도를 더해 최종 발사 각도를 계산
                // 예를 들어, projectileCount가 12이면 각 투사체들은 (0, 30, 60 ... 360)각도를 갖는다
                // 여기에 무작위로 선택된 시작 각도를 더해(0 ~ 360) 범위 내에서 균등하게 분포된 발사 각도를 만든다
                float currentAngle = (angleStep * j) + randomStartAngle;

                // Z축을 기준으로 회전
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

                // 회전 값(rotation)을 이용해 실제 날아갈 방향 벡터 계산
                // Vector2.right는 (1, 0) 벡터, 오른쪽 방향을 나타냄
                // 오른쪽 방향을 currentAngle만큼 회전시킨 새로운 방향 벡터를 만들어 냄
                Vector2 direction = rotation * Vector2.right;

                //GameObject projectileGO = Object.Instantiate(projectilePrefab, boss.transform.position, rotation);
                GameObject projectileGO = ObjectPoolingManager.Instance.Get(projectilePrefab, boss.transform.position, rotation);

                BossProjectileController projectile = projectileGO.GetComponent<BossProjectileController>();
                if (projectile != null)
                {
                    projectile.Init(direction, projectileSpeed, projectileDuration, projectilePower, targetLayer,
                                    applyKnockback, knockbackPower, knockbackDuration);
                }
            }

            // 마지막 발사가 아니면, 정해진 시간만큼 대기
            if (i < burstCount - 1)
            {
                yield return new WaitForSeconds(delayBetweenBursts);
            }
        }

        // 모든 발사가 끝나면 다음 상태(Idle)로 전환
        boss.ChangeState(new BossIdleState(Cooldown));
    }
}
