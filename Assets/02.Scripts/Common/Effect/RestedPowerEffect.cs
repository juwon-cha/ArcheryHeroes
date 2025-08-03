using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Effect/RestedPowerEffect")]
public class RestedPowerEffect : EffectSO
{
    private PlayerController playerController;
    private WeaponHandler weaponHandler;

    [SerializeField] private GameObject powerEffectPrefab;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [SerializeField] private float maxRestedTime = 5f; // 최대 휴식 시간
    [Range(0f, 2f)]
    [SerializeField] private float powerMultiplier = 0.5f; // 0.5 = 50% 데미지 증가
    [Range(0f, .9f)]
    [SerializeField] private float delayMultiplier = 0.5f; // 0.5 = 50% 공격 속도 감소
    private float currentRestedTime = 0f;
    private bool wasIdle = false; // 이전 프레임 상태 기억
    private ParticleSystem powerEffect;

    private float originalPower;
    private float originalDelay;


    public override void Initialize()
    {
        playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        currentRestedTime = 0f;
        wasIdle = false;
    }

    public override void Execute(EffectContext effectContext = null)
    {
        bool isIdle = IsIdle();

        if (isIdle)
        {
            if (!wasIdle)
            {
                weaponHandler = playerController.GetComponentInChildren<WeaponHandler>(true);

                originalPower = weaponHandler.Power; // 원래 힘 저장
                originalDelay = weaponHandler.Delay; // 원래 딜레이 저장
                powerEffect = ObjectPoolingManager.Instance.Get(powerEffectPrefab, playerController.transform.position).GetComponent<ParticleSystem>();
                var main = powerEffect.main;
                main.loop = true; // 파티클 효과를 다시 시작
            }

            currentRestedTime += Time.deltaTime;
            if (currentRestedTime > maxRestedTime)
                currentRestedTime = maxRestedTime; // 최대 휴식 시간 초과 방지

            ApplyRestedPower();
        }
        else
        {
            if (wasIdle) // 이번 프레임에 처음으로 Idle이 아닌 상태가 됐을 때만
            {
                var main = powerEffect.main;
                main.loop = false; // 파티클 효과를 다시 시작
                currentRestedTime = 0f;
                ApplyRestedPower();
            }
        }

        wasIdle = isIdle; // 상태 갱신
    }

    public override void Deactivate()
    {
        if (powerEffect != null)
        {
            var main = powerEffect.main;
            main.loop = false; // 파티클 효과 중지
        }
        
        if (weaponHandler != null)
        {
            weaponHandler.Power = originalPower; // 원래 힘으로 복원
            weaponHandler.Delay = originalDelay; // 원래 딜레이로 복원
        }

        currentRestedTime = 0f; // 휴식 시간 초기화
        wasIdle = false; // 상태 초기화
    }


    void ApplyRestedPower()
    {
        float t = Mathf.Clamp(currentRestedTime / maxRestedTime, 0f, 1f);
        weaponHandler.Power = originalPower * (1f + t * powerMultiplier);
        weaponHandler.Delay = originalDelay * (1f - t * delayMultiplier);

        // Debug.Log($"Rested Power: {weaponHandler.Power}, Delay: {weaponHandler.Delay}");
        SetEffectColor(t);
    }

    void SetEffectColor(float t)
    {
        if (powerEffect == null) return;

        var main = powerEffect.main;
        main.startColor = Color.Lerp(startColor, endColor, t);
    }


    bool IsIdle()
    {
        return playerController.MovementDirection == Vector2.zero;
    }

}
