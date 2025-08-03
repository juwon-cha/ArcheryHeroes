using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Effect/ResurrectEffect")]
public class ResurrectEffect : EffectSO
{
    [SerializeField] GameObject resurrectEffect; // 부활 이펙트 프리팹
    [Range(0f, 1f)]
    [SerializeField] float resurrectChance = 0.5f; // 50% 확률로 부활

    private ResourceController playerResourceController;
    private bool isInitialized = false;

    public override void Initialize()
    {
        playerResourceController = GameManager.Instance.Player.GetComponent<ResourceController>();
        isInitialized = false;
    }

    public override void Execute(EffectContext effectContext = null)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            playerResourceController.AddHealthChangeEvent(OnHealthChanged);
        }
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        if (currentHealth > 0) return;
        if (Random.value > resurrectChance) return; // 부활 확률 체크

        // 부활 이펙트 생성
        ObjectPoolingManager.Instance.Get(resurrectEffect, playerResourceController.transform.position);
        playerResourceController.Resurrect(); // 체력을 최대치로 회복
    }

    public override void Deactivate()
    {
        isInitialized = false;
        playerResourceController.RemoveHealthChangeEvent(OnHealthChanged);
    }
}
