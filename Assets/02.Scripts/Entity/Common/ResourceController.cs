using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float _healthChangeDelay = 0.5f; // 체력 변경 딜레이(무적)

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    // 변화를 가진 시간 저장
    private float timeSinceLastHealthChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => statHandler.Health;

    public AudioClip DamageClip;

    private Action<float, float> OnChangeHealth;

    private void Awake()
    {
        baseController = GetComponent<BaseController>();
        if (baseController == null)
        {
            Debug.LogError("BaseController component is missing on " + gameObject.name);
        }

        statHandler = GetComponent<StatHandler>();
        if (statHandler == null)
        {
            Debug.LogError("StatHandler component is missing on " + gameObject.name);
        }

        animationHandler = GetComponent<AnimationHandler>();
        if (animationHandler == null)
        {
            Debug.LogError("AnimationHandler component is missing on " + gameObject.name);
        }
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    private void Update()
    {
        // 체력 변경 딜레이 체크
        if (timeSinceLastHealthChange < _healthChangeDelay)
        {
            timeSinceLastHealthChange += Time.deltaTime;
            if (timeSinceLastHealthChange >= _healthChangeDelay)
            {
                animationHandler.InvincibilityEnd(); // 무적 상태 해제
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastHealthChange < _healthChangeDelay)
        {
            return false; // 체력 변경이 없거나 무적 상태라면 변경하지 않음
        }

        timeSinceLastHealthChange = 0f; // 체력 변경 시간 초기화

        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth; // 최대 체력 초과 방지
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; // 최소 체력 0으로 제한

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth); // 체력 변경 이벤트 호출

        if (change < 0)
        {
            animationHandler.Damage(); // 데미지 애니메이션 실행

            if (DamageClip != null)
            {
                // 데미지 사운드 재생
            }
        }

        if (CurrentHealth <= 0)
        {
            Death();
        }

        return true;
    }

    private void Death()
    {
        baseController.OnDead();
    }

    public void AddHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth += action;
    }

    public void RemoveHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth -= action;
    }
}
