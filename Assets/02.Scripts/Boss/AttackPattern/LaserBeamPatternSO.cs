using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLaserBeamPattern", menuName = "Boss/Attack Patterns/Laser Beam")]
public class LaserBeamPatternSO : BossAttackSO
{
    [Header("레이저 설정")]
    [SerializeField] private float chargeDuration = 1f; // 충전 시간
    [SerializeField] private float laserDuration = 3f;  // 레이저 발사 지속 시간
    [SerializeField] private float laserDamage = 10f;   // 초당 대미지

    [Header("레이저 외형")]
    [SerializeField] private Material laserMaterial;    // 레이저를 그릴 머티리얼
    [SerializeField] private float laserWidth = 0.2f;   // 레이저 두께

    public override void Execute(BossController boss)
    {
        boss.StartCoroutine(FireLaserCoroutine(boss));
    }

    // 실제 레이저 발사 로직을 담고 있는 코루틴
    private IEnumerator FireLaserCoroutine(BossController boss)
    {
        // LineRenderer 준비
        LineRenderer laserRenderer = boss.GetComponent<LineRenderer>();
        if (laserRenderer == null)
        {
            // 만약 LineRenderer가 없다면, 새로 추가해줍니다.
            laserRenderer = boss.gameObject.AddComponent<LineRenderer>();
        }

        // 레이저 외형 설정
        laserRenderer.enabled = true;               // 활성화 상태 보장
        laserRenderer.positionCount = 2;            // 점 개수 보장
        laserRenderer.material = laserMaterial;
        laserRenderer.startWidth = laserWidth;
        laserRenderer.endWidth = laserWidth;
        //laserRenderer.sortingLayerName = "Laser";   // 필요하다면 Layer 이름 지정
        //laserRenderer.sortingOrder = 120;           // 필요하다면 순서 지정

        // 충전 단계
        Debug.Log($"{boss.name}이(가) {AttackName} 충전 시작!");
        // boss.animator.SetTrigger("ChargeLaser"); // 충전 애니메이션이 있다면...
        yield return new WaitForSeconds(chargeDuration);

        // 레이저 발사 단계
        Debug.Log("레이저 발사!");
        laserRenderer.enabled = true;

        float timer = laserDuration;
        while (timer > 0)
        {
            // 매 프레임 플레이어를 향하도록 방향을 다시 계산 (유도 레이저)
            Vector2 targetDirection = (boss.Target.position - boss.transform.position).normalized;

            Vector3 startPos = new Vector3(boss.transform.position.x, boss.transform.position.y, 0f);
            laserRenderer.SetPosition(0, startPos);

            // 레이저의 시작점은 항상 보스의 위치
            //laserRenderer.SetPosition(0, boss.transform.position);

            // 레이저의 끝점을 Raycast를 이용해 결정
            RaycastHit2D hit = Physics2D.Raycast(boss.transform.position, targetDirection, 100f);

            if (hit.collider != null)
            {
                // 무언가에 맞았다면, 맞은 지점이 끝점
                laserRenderer.SetPosition(1, hit.point);
                if (hit.collider.CompareTag("Player"))
                {
                    // 맞은 것이 플레이어라면 대미지를 줌
                    // hit.collider.GetComponent<PlayerHealth>().TakeDamage(laserDamage * Time.deltaTime);
                }
            }
            else
            {
                // 아무것도 맞지 않았다면, 최대 사거리까지 레이저를 그림
                laserRenderer.SetPosition(1, (Vector2)boss.transform.position + targetDirection * 100f);
            }

            timer -= Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        Debug.Log("레이저 패턴 종료.");
        // 단순히 비활성화하는 대신, 점의 개수를 0으로 만들어 확실하게 라인을 지웁니다.
        if (laserRenderer != null)
        {
            laserRenderer.positionCount = 0;
        }
        // enabled를 꺼도 좋지만, positionCount를 0으로 하는 것이 더 확실한 방법입니다.
        laserRenderer.enabled = false;

        // 공격이 완전히 끝났으므로, 다음 상태로 전환
        boss.ChangeState(new BossIdleState(Cooldown));
    }
}