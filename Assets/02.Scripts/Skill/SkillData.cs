using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    public float cooldown; // 쿨타임 (초 단위)
    public List<ConditionSO> conditions; // 스킬 사용 조건들
    public List<EffectSO> effects; // 스킬 효과들

    public void Initialize()
    {
        foreach (var condition in conditions)
            condition.Initialize(); // 각 조건 초기화
        foreach (var effect in effects)
            effect.Initialize(); // 각 효과 초기화
    }
}
