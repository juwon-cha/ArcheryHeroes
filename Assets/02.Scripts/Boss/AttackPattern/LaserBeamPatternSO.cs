using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "NewLaserBeamPattern", menuName = "Boss/Attack Patterns/Laser Beam")]
public class LaserBeamPatternSO : BossAttackSO
{
    [Header("레이저 설정")]
    [SerializeField] private float rotationDuration = 8f; // 한 바퀴(360도)를 도는 데 걸리는 시간
    [SerializeField] private float laserLength = 20f;     // 각 레이저의 고정 길이
    [SerializeField] private float laserWidth = 0.5f;     // 레이저의 두께
    [SerializeField] private float laserDamage = 5f;      // 초당 대미지

    [Header("레이저 외형")]
    [SerializeField] private GameObject laserSpritePrefab; // 사용할 레이저 스프라이트 프리팹

    public override void Execute(BossController boss)
    {
        boss.StartCoroutine(RotateLasersCoroutine(boss));
    }

    private IEnumerator RotateLasersCoroutine(BossController boss)
    {
        if (laserSpritePrefab == null)
        {
            Debug.LogError($"{AttackName}: laserSpritePrefab이 할당되지 않았습니다!");
            boss.ChangeState(new BossIdleState(Cooldown));
            yield break;
        }

        // 3개의 레이저 생성
        Debug.Log($"{boss.name}이(가) {AttackName} 시전!");
        List<GameObject> lasers = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject laser = Object.Instantiate(laserSpritePrefab, boss.transform.position, Quaternion.identity);
            lasers.Add(laser);
        }

        // 정해진 시간 동안 회전
        float timer = rotationDuration;
        while (timer > 0)
        {
            // 현재 회전 진행률 계산 (0.0 -> 1.0)
            float progress = 1f - (timer / rotationDuration);
            // 전체 360도 중 현재의 기본 각도 계산
            float baseAngle = 360f * progress;

            // 3개의 레이저를 각각 업데이트
            for (int i = 0; i < lasers.Count; i++)
            {
                // 기본 각도에서 각 레이저의 고유한 오프셋(0, 120, 240도)을 더함
                float currentAngle = baseAngle + (i * 120f);

                // 위치는 항상 보스 중심으로 고정
                lasers[i].transform.position = boss.transform.position;
                // 계산된 각도로 회전
                lasers[i].transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                // 고정된 길이와 너비로 스케일 설정
                lasers[i].transform.localScale = new Vector3(laserLength, laserWidth, 1f);

                // 충돌 처리
                Vector2 origin = boss.transform.position;
                Vector2 direction = lasers[i].transform.right; // 현재 레이저가 향하는 방향
                RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, laserLength);
                Debug.DrawRay(origin, direction * laserLength, Color.red);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("플레이어 피격!");
                        // hit.collider.GetComponent<PlayerHealth>().TakeDamage(laserDamage * Time.deltaTime);
                    }
                }
            }

            timer -= Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        Debug.Log("회전 레이저 패턴 종료");
        foreach (GameObject laser in lasers)
        {
            Object.Destroy(laser);
        }

        boss.ChangeState(new BossIdleState(Cooldown));
    }
}