using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChargePattern", menuName = "Boss/Attack Patterns/Charge Attack")]
public class ChargePatternSO : BossAttackSO
{
    [Header("돌진 설정")]
    [SerializeField] private float aimDuration = 1.2f;      // 인디케이터가 표시되는 조준 시간
    [SerializeField] private float chargeSpeed = 25f;       // 돌진 속도
    [SerializeField] private float chargeDistance = 5f;    // 돌진할 거리
    [SerializeField] private float power = 2f;              // 돌진 추가 공격력

    [Header("인디케이터 설정")]
    [SerializeField] private GameObject indicatorPrefab;    // 돌진 경로를 표시할 프리팹
    [SerializeField] private float chargeWidth = 2f;        // 돌진 경로의 폭

    [Header("충돌 설정")]
    [SerializeField] private LayerMask wallLayer; // 벽 감지 레이어 마스크

    public override void Execute(BossController boss)
    {
        boss.StartCoroutine(ChargeCoroutine(boss));
    }

    private IEnumerator ChargeCoroutine(BossController boss)
    {
        // 조준 및 인디케이터 표시
        Debug.Log($"{boss.name}이(가) {AttackName} 조준 시작!");

        // 플레이어를 향하는 방향을 한 번만 계산
        Vector2 direction = boss.DirectionToTarget(); // 보스에서 플레이어를 향하는 순수한 방향
        boss.LookDirection = direction; // 보스의 LookDirection 업데이트

        // 벽 감지 및 거리 조절 로직
        // BoxCast로 전방의 벽을 확인
        RaycastHit2D hit = Physics2D.BoxCast(
            boss.transform.position,            // 발사 위치
            boss.BoxCollider.size,              // 보스 콜라이더의 크기
            0f,                                 // 회전 없음
            direction,                          // 돌진 방향
            chargeDistance,                     // 최대 감지 거리
            wallLayer                           // 감지할 레이어
        );

        float actualChargeDistance;
        if (hit.collider != null)
        {
            actualChargeDistance = hit.distance;
        }
        else
        {
            actualChargeDistance = chargeDistance;
        }

        GameObject indicator = null;
        if (indicatorPrefab != null)
        {
            // 인디케이터 생성
            indicator = Object.Instantiate(indicatorPrefab, boss.transform.position, Quaternion.identity);

            // 인디케이터의 방향을 돌진 방향으로 설정
            // 방향 벡터(direction) 사용해 화면상의 각도(degree) 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //계산된 각도만큼 인디케이터 회전
            indicator.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 인디케이터의 크기를 돌진 거리와 폭에 맞게 조절
            indicator.transform.localScale = new Vector3(actualChargeDistance, chargeWidth, 1f);

            // 인디케이터의 위치를 보스와 돌진 끝점의 중앙으로 이동
            indicator.transform.position = (Vector2)boss.transform.position + direction * (actualChargeDistance / 2f);
        }

        // 설정된 조준 시간만큼 대기
        boss.StopMovement();
        yield return new WaitForSeconds(aimDuration);

        if (boss == null)
        {
            // 보스가 사라졌다면 인디케이터도 정리
            if (indicator != null)
            {
                Object.Destroy(indicator);
            }
            yield break; // 코루틴 즉시 종료
        }

        // 돌진 실행
        if (indicator != null)
        {
            // 인디케이터 파괴
            Object.Destroy(indicator);
        }

        Debug.Log("돌진!");

        boss.AdditionalPower = power; // 돌진 시 추가 공격력 설정

        // 돌진에 걸리는 시간 계산
        float chargeActualDuration = actualChargeDistance / chargeSpeed;

        // 물리 엔진을 통해 보스에게 속도 부여
        boss.Rigidbody.velocity = direction * chargeSpeed;

        // 계산된 시간만큼 대기
        yield return new WaitForSeconds(chargeActualDuration);

        // 돌진 중에 보스가 파괴되었는지 확인
        if (boss == null)
        {
            yield break; // 코루틴 즉시 종료
        }

        // 종료 단계
        Debug.Log("돌진 종료.");

        // 돌진이 끝나면 멈춤
        boss.StopMovement();

        boss.AdditionalPower = 0f; // 추가 공격력 초기화

        boss.ChangeState(new BossIdleState(Cooldown));
    }
}
