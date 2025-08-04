using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementEffectManager : Singleton<ElementEffectManager>
{
    [SerializeField] private GameObject lightningEffectPrefab;
    [SerializeField] private float lightningDetectionRadius = 3f; // 전기 효과 범위
    [SerializeField] private int lightningCount = 2; // 전기 효과 적용 횟수

    public void ApplyElementEffect(ResourceController caster, float change, ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire:
                break;
            case ElementType.Poison:
                break;
            case ElementType.Lightning:
                ObjectPoolingManager.Instance.Get(lightningEffectPrefab, caster.transform.position);
                ApplyLightningEffect(caster, change, lightningCount);
                break;
        }
    }

    private void ApplyLightningEffect(ResourceController caster, float change, int count)
    {
        if (count <= 0) return;
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(caster.transform.position, lightningDetectionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == caster.gameObject) continue;
            if (hitCollider.gameObject == GameManager.Instance.Player) continue;

            ResourceController nearController = hitCollider.GetComponent<ResourceController>();
            if (nearController != null)
            {
                ObjectPoolingManager.Instance.Get(lightningEffectPrefab, nearController.transform.position);
                nearController.ChangeHealth(change);
                ApplyLightningEffect(nearController, change, --count);
            }
        }
    }

}
