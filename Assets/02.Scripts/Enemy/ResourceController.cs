using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float _healthChangeDelay = 0.5f; // ü�� ���� ������(����)

    private EnemyController enemyController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    // ��ȭ�� ���� �ð� ����
    private float timeSinceLastHealthChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => statHandler.Health;

    public AudioClip DamageClip;

    private Action<float, float> OnChangeHealth;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        if (enemyController == null)
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
        // ü�� ���� ������ üũ
        if (timeSinceLastHealthChange < _healthChangeDelay)
        {
            timeSinceLastHealthChange += Time.deltaTime;
            if (timeSinceLastHealthChange >= _healthChangeDelay)
            {
                animationHandler.InvincibilityEnd(); // ���� ���� ����
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastHealthChange < _healthChangeDelay)
        {
            return false; // ü�� ������ ���ų� ���� ���¶�� �������� ����
        }

        timeSinceLastHealthChange = 0f; // ü�� ���� �ð� �ʱ�ȭ

        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth; // �ִ� ü�� �ʰ� ����
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; // �ּ� ü�� 0���� ����

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth); // ü�� ���� �̺�Ʈ ȣ��

        if (change < 0)
        {
            animationHandler.Damage(); // ������ �ִϸ��̼� ����

            if (DamageClip != null)
            {
                // ������ ���� ���
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
        enemyController.OnDead();
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
