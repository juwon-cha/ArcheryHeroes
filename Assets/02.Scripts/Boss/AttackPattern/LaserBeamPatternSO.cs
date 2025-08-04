using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "NewLaserBeamPattern", menuName = "Boss/Attack Patterns/Laser Beam")]
public class LaserBeamPatternSO : BossAttackSO
{
    [Header("레이저 설정")]
    [SerializeField] private float laserCount = 3f;       // 레이저 개수
    [SerializeField] private float rotationDuration = 8f; // 한 바퀴(360도)를 도는 데 걸리는 시간
    [SerializeField] private float laserLength = 20f;     // 각 레이저의 고정 길이
    [SerializeField] private float laserWidth = 0.5f;     // 레이저의 두께
    [SerializeField] private float laserDamage = 5f;      // 레이저 데미지

    [Header("레이저 외형")]
    [SerializeField] private GameObject laserSpritePrefab; // 사용할 레이저 스프라이트 프리팹
    [SerializeField] private LayerMask wallLayer; // 레이저가 충돌할 벽 레이어

    public override void Execute(BossController boss)
    {
        boss.StartCoroutine(RotateLasersCoroutine(boss));
    }

    private IEnumerator RotateLasersCoroutine(BossController boss)
    {
        if (laserSpritePrefab == null || boss == null)
        {
            Debug.LogError($"{AttackName}: laserSpritePrefab 또는 Boss가 할당되지 않았습니다!");
            boss.ChangeState(new BossIdleState(Cooldown));
            yield break;
        }

        // 레이저 생성
        Debug.Log($"{boss.name}이(가) {AttackName} 시전!");
        List<GameObject> lasers = new List<GameObject>();
        for (int i = 0; i < laserCount; i++)
        {
            GameObject laser = Object.Instantiate(laserSpritePrefab, boss.transform.position, Quaternion.identity);
            lasers.Add(laser);
        }

        // 각 레이저 사이의 각도 계산
        float angleBetweenLasers = 360f / laserCount;

        // 정해진 시간 동안 회전
        float timer = rotationDuration;
        while (timer > 0)
        {
            // 보스와 타겟의 생존 여부 확인
            if (boss == null || boss.Target == null)
            {
                // 루프가 중단되면 생성된 레이저들을 모두 파괴
                foreach (GameObject laser in lasers)
                {
                    if (laser != null)
                    {
                        Object.Destroy(laser);
                    }
                }
                yield break; // 코루틴 즉시 종료
            }

            // 현재 회전 진행률 계산 (0.0 -> 1.0)
            float progress = 1f - (timer / rotationDuration);
            // 전체 360도 중 현재의 기본 각도 계산
            float baseAngle = 360f * progress;

            // 레이저를 각각 업데이트
            for (int i = 0; i < lasers.Count; i++)
            {
                // 미리 계산한 각도를 사용하여 현재 각도 계산
                float currentAngle = baseAngle + (i * angleBetweenLasers);

                // 위치는 항상 보스 중심으로 고정
                lasers[i].transform.position = boss.transform.position;
                // 계산된 각도로 회전
                lasers[i].transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                // 레이저 길이 동적 계산
                Vector2 origin = boss.transform.position;
                Vector2 direction = lasers[i].transform.right; // 레이저의 정면 방향
                float currentMaxDistance = laserLength; // 기본 최대 길이

                // 장애물 레이어에 Raycast를 쏴서 부딪히는지 확인
                RaycastHit2D obstacleHit = Physics2D.Raycast(origin, direction, laserLength, wallLayer);

                if (obstacleHit.collider != null)
                {
                    // 부딪힌 오브젝트가 Tilemap인지 확인
                    if (obstacleHit.collider.TryGetComponent<Tilemap>(out Tilemap tilemap))
                    {
                        // 레이가 타일 안으로 살짝 들어간 위치 계산 (정확한 타일 식별을 위해)
                        Vector3 insidePoint = obstacleHit.point - (direction * 0.01f);

                        // 월드 좌표를 타일맵의 셀 좌표로 변환
                        Vector3Int hitCellPosition = tilemap.WorldToCell(insidePoint);

                        // 셀 좌표를 이용해 해당 타일의 월드 좌표 기준 중앙 위치를 가져옴
                        Vector3 cellCenter = tilemap.GetCellCenterWorld(hitCellPosition);

                        // 보스 위치부터 타일 중앙까지의 거리를 레이저의 최종 길이로 설정
                        currentMaxDistance = Vector2.Distance(origin, cellCenter);
                    }
                    else
                    {
                        // Tilemap이 아닌 다른 장애물(일반 벽 등)에 부딪혔을 경우
                        currentMaxDistance = obstacleHit.distance;
                    }
                }

                // 계산된 길이로 레이저 스케일 조절
                lasers[i].transform.localScale = new Vector3(currentMaxDistance, laserWidth, 1f);

                // 충돌 처리
                RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, laserLength);
                Debug.DrawRay(origin, direction * laserLength, Color.red);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.layer == boss.Target.gameObject.layer)
                    {
                        Debug.Log("플레이어 피격!");

                        // 데미지 처리 (프레임당 데미지)
                        ResourceController targetResource = hit.collider.GetComponent<ResourceController>();
                        if (targetResource != null)
                        {
                            // Time.deltaTime을 곱하여 프레임에 따른 피해량 계산
                            float damageThisFrame = laserDamage * Time.deltaTime;
                            targetResource.ChangeHealth(-damageThisFrame);
                        }

                        // 넉백 처리
                        if (boss.ApplyKnockback)
                        {
                            BaseController controller = hit.collider.GetComponent<BaseController>();
                            if (controller != null)
                            {
                                // BossController에서 넉백 정보를 가져와 전달
                                controller.ApplyKnockBack(boss.transform, boss.KnockbackPower, boss.KnockbackDuration);
                            }
                        }
                    }
                }
            }

            timer -= Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        Debug.Log("회전 레이저 패턴 종료");
        foreach (GameObject laser in lasers)
        {
            if (laser != null)
            {
                Object.Destroy(laser);
            }
        }

        if(boss != null)
        {
            boss.ChangeState(new BossIdleState(Cooldown));
        }
    }
}