using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Effect/CircleEffect")]
public class CircleEffect : EffectSO
{
    public GameObject effectPrefab; // 생성할 효과 프리팹
    public int count = 12; // 생성할 효과의 개수
    public float radius = 1f; // 효과가 생성될 반경

    public override void Initialize() { }

    public override void Execute(EffectContext effectContext = null)
    {
        // 원형으로 effect 생성
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count; // 각도 계산
            Vector3 position = effectContext.Position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            ObjectPoolingManager.Instance.Get(effectPrefab, position, Quaternion.identity);
        }
    }
}
