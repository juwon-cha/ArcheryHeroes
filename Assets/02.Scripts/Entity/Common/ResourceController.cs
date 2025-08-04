using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class ResourceController : MonoBehaviour
{
    [SerializeField] protected float _healthChangeDelay = 0.5f; // 체력 변경 딜레이(무적)
    [SerializeField] protected SoundDataSO damageSFX; // 데미지 효과음
    [SerializeField] protected SoundDataSO deathSFX; // 사망 효과음
    [SerializeField] protected GameObject damageEffect; // 데미지 이펙트
    [SerializeField] protected float damageEffectRandomOffsetRange = 0.5f; // 데미지 이펙트 위치 랜덤 오프셋 범위

    protected BaseController baseController;
    protected StatHandler statHandler;
    protected AnimationHandler animationHandler;

    // 변화를 가진 시간 저장
    private float timeSinceLastHealthChange = float.MaxValue;

    public float CurrentHealth { get; protected set; }
    public float MaxHealth => statHandler.Health;

    protected Action<float, float> OnChangeHealth;

    protected virtual void Awake()
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

    private void OnEnable()
    {
        HPBarManager.Instance.CreateHPBar(this); // HPBar 생성
    }

    private void OnDisable()
    {
        HPBarManager.Instance.RemoveHPBar(this); // HPBar 제거
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

    public void Resurrect()
    {
        CurrentHealth = MaxHealth;
        timeSinceLastHealthChange = 0f; // 무적 상태 시작
    }

    public void Heal(float change)
    {
        if (change <= 0) return;
        Debug.Log($"Heal: {change}"); // Heal 로그 출력

        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth; // 최대 체력 초과 방지
        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth); // 체력 변경 이벤트 호출
    }

    public bool ChangeHealth(float change, ElementType elementType = ElementType.None)
    {
        if (change == 0 || timeSinceLastHealthChange < _healthChangeDelay)
        {
            return false; // 체력 변경이 없거나 무적 상태라면 변경하지 않음
        }

        timeSinceLastHealthChange = 0f; // 체력 변경 시간 초기화

        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth; // 최대 체력 초과 방지
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; // 최소 체력 0으로 제한

        ElementEffectManager.Instance.ApplyElementEffect(this, change, elementType); // 엘리먼트 효과 적용
        DamageTextManager.Instance.ShowDamageText((int)-change, transform.position); // 데미지 텍스트 표시
        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth); // 체력 변경 이벤트 호출

        Vector2 offset = Random.insideUnitCircle * damageEffectRandomOffsetRange; // 랜덤 위치 오프셋
        Vector2 pos = transform.position + (Vector3)offset; // 현재 위치에 오프셋 추가
        ObjectPoolingManager.Instance.Get(damageEffect, pos);

        if (CurrentHealth <= 0)
        {
            AudioManager.Instance.PlaySFX(deathSFX); // 사망 사운드 재생
            Death();
        }
        else if (change < 0)
        {
            AudioManager.Instance.PlaySFX(damageSFX); // 데미지 사운드 재생
            animationHandler.Damage(); // 데미지 애니메이션 실행
        }

        return true;
    }


    protected virtual void Death()
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

    // 정진규 추가
    public void RestoreAndReset()
    {
        // 1. 피 회복
        CurrentHealth = MaxHealth;

        // 2. 무적 타이머 초기화
        timeSinceLastHealthChange = float.MaxValue;

        // 3. 피격 애니메이션 해제
        animationHandler.InvincibilityEnd();

        // 4. 체력 변경 이벤트 호출
        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
    }
}
