using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Effect/RestedPowerEffect")]
public class RestedPowerEffect : EffectSO
{
    private PlayerController playerController;

    [SerializeField] private GameObject powerEffectPrefab;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [SerializeField] private float maxRestedTime = 5f; // 최대 휴식 시간
    [SerializeField] private float restedPowerMultiplier = 1.5f; // 1.5 = 150%
    private float currentRestedTime = 0f;
    private bool wasIdle = false; // 이전 프레임 상태 기억
    private ParticleSystem powerEffect;


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

        currentRestedTime = 0f; // 휴식 시간 초기화
        wasIdle = false; // 상태 초기화
    }


    void ApplyRestedPower()
    {
        float t = Mathf.Clamp(currentRestedTime / maxRestedTime, 0f, 1f);
        float restedPower = t * restedPowerMultiplier; // 휴식 시간에 따른 힘 배수 적용
        SetEffectColor(t);
        // playerController.ApplyRestedPower(restedPower);
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
