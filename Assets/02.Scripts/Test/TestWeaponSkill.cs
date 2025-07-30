using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/TestWeaponSkill")]
public class TestWeaponSkill : SkillBase
{
    public GameObject effect;
    public int count = 12; // 생성할 효과의 개수
    public float radius = 1f; // 효과가 생성될 반경

    protected override void OnUse(GameObject player)
    {
        Debug.Log($"Skill Used: {skillName}");

        // 원형으로 effect 생성
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count; // 각도 계산
            Vector3 position = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            ObjectPoolingManager.Instance.Get(effect, position, Quaternion.identity);
        }
    }

}
