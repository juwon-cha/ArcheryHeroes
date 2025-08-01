using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("데미지 설정")]
    [SerializeField] private float damagePerTick = 5f;    // 한 번의 틱마다 입힐 대미지 양
    [SerializeField] private float damageInterval = 0.5f; // 데미지가 들어가는 간격 (초)

    private bool isPlayerInside = false; // 플레이어가 현재 장판 안에 있는지 여부
    private float damageTimer;           // 다음 데미지 틱까지 시간 재는 타이머
    private ResourceController playerResourceController;

    private void Start()
    {
        // 처음에 즉시 데미지 줄 수 있도록 타이머 0으로 설정
        damageTimer = 0f;
    }

    private void Update()
    {
        // 플레이어가 장판 안에 있을 때만 데미지 로직을 실행
        if (isPlayerInside)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                // 데미지를 줄 시간이 되면
                ApplyDamage();
                // 다음 데미지 시간까지 타이머 초기화
                damageTimer = damageInterval;
            }
        }
    }

    private void ApplyDamage()
    {
        if (playerResourceController != null)
        {
            Debug.Log($"플레이어에게 {damagePerTick}의 지속 피해!");

            playerResourceController.ChangeHealth(-damagePerTick);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            // 플레이어의 ResourceController 저장
            playerResourceController = collision.GetComponent<ResourceController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerResourceController = null;
        }
    }
}