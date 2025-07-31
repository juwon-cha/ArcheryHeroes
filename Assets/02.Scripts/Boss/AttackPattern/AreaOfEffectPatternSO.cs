using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewGroundAoEPattern", menuName = "Boss/Attack Patterns/Ground AoE")]
public class AreaOfEffectPatternSO : BossAttackSO
{
    [Header("장판 공격 설정")]
    [SerializeField] private int areaCount = 3;                // 생성할 장판 개수
    [SerializeField] private float spawnRadius = 10f;          // 플레이어 주변 어느 반경 내에 생성할지
    [SerializeField] private float warningDuration = 1.5f;     // 경고 시간
    [SerializeField] private GameObject warningIndicatorPrefab; // 경고 표시 프리팹
    [SerializeField] private GameObject damageZonePrefab;     // 실제 대미지 장판 프리팹
    [SerializeField] private float damageZoneLifetime = 5f;    // 생성된 장판 지속 시간

    [Header("장판 외형 설정")]
    [SerializeField] private float areaSize = 3f; // 장판 크기

    public override void Execute(BossController boss)
    {
        // 코루틴을 시작시켜 시간차를 둔 행동을 처리합니다.
        boss.StartCoroutine(GroundAoECoroutine(boss));
    }

    private IEnumerator GroundAoECoroutine(BossController boss)
    {
        Tilemap tilemap = boss.GroundTilemap;

        if (warningIndicatorPrefab == null || damageZonePrefab == null || tilemap == null)
        {
            Debug.LogError($"{AttackName}: Prefab이 할당되지 않았습니다!");
            boss.ChangeState(new BossIdleState(Cooldown));
            yield break;
        }

        Bounds tilemapBounds = tilemap.localBounds; // 타일맵의 로컬 경계를 가져옴

        Debug.Log($"{boss.name}이(가) {AttackName} (총 {areaCount}개) 시전!");

        // 모든 경고 장판 위치 계산 및 동시 생성
        // 공격 시작 시점의 플레이어 위치를 기준으로 삼음
        Vector3 centerPosition = boss.Target.position;

        // 경고 장판 리스트
        List<GameObject> warningIndicators = new List<GameObject>();

        // 경고 장판 생성
        for (int i = 0; i < areaCount; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            bool isValidPosition = false;
            int maxAttempts = 50; // 무한 루프 방지를 위한 최대 시도 횟수

            // 유효한 위치를 찾을 때까지 최대 maxAttempts 만큼 반복
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // 플레이어 주변에 랜덤 위치를 생성
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                spawnPosition = centerPosition + (Vector3)randomOffset;

                // 생성된 위치가 타일맵 경계 안에 있는지 확인
                if (tilemapBounds.Contains(spawnPosition))
                {
                    isValidPosition = true;
                    break; // 유효한 위치를 찾았으므로 재시도 루프 탈출
                }
            }

            // 유효한 위치를 찾은 경우에만 경고 장판 생성
            if (isValidPosition)
            {
                GameObject warningIndicator = Object.Instantiate(warningIndicatorPrefab, spawnPosition, Quaternion.identity);
                warningIndicator.transform.localScale = Vector3.one * areaSize;
                warningIndicators.Add(warningIndicator);
            }
            else
            {
                Debug.LogWarning($"{AttackName}: 유효한 장판 생성 위치를 찾는 데 실패했습니다.");
            }
        }

        // 모든 경고를 보여주며 기다림
        yield return new WaitForSeconds(warningDuration);

        // 경고 장판을 실제 대미지 장판으로 교체
        foreach (GameObject indicator in warningIndicators)
        {
            // 실제 대미지를 주는 장판을 경고 표시기 위치에 생성
            GameObject damageZone = Object.Instantiate(damageZonePrefab, indicator.transform.position, Quaternion.identity);

            damageZone.transform.localScale = Vector3.one * areaSize;

            // 플레이어가 장판 위에 있으면 지속 피해 받음
            //StatHandler playerStatHandler = boss.Target.GetComponent<StatHandler>();
            //if (playerStatHandler != null)
            //{
            //    // 장판이 플레이어에게 지속 피해를 주도록 설정
            //    damageZone.GetComponent<DamageZone>().Initialize(playerStatHandler, boss.StatHandler);
            //}
            //else
            //{
            //    Debug.LogWarning("플레이어의 StatHandler가 할당되지 않았습니다!");
            //}

            Object.Destroy(damageZone, damageZoneLifetime); // 일정 시간 후 자동 파괴

            // 역할을 다한 경고 표시기 파괴
            Object.Destroy(indicator);
        }

        boss.ChangeState(new BossIdleState(Cooldown));
    }
}
