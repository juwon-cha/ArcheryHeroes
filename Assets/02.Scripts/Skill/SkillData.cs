using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    public SkillDataSO skillSO;  // 기준이 되는 SO
    public int currentLevel;     // 현재 레벨

    public string SkillName { get => skillSO.skillName; }
    public string Description { get => skillSO.description; }
    public Sprite Icon { get => skillSO.icon; }
    public int MaxLevel { get => skillSO.MaxLevel; }
    public List<StatModifier> Modifiers { get => skillSO.modifiers; }

    public SkillData(SkillDataSO skillSO)
    {
        this.skillSO = skillSO;
        currentLevel = 0;
    }

    public bool IsMaxLevel()
    {
        return currentLevel >= skillSO.MaxLevel;
    }

    public void LevelUp()
    {
        if (!skillSO.isLevelUpEnabled) return;

        if (!IsMaxLevel())
        {
            currentLevel++;
        }
    }

    public StatModifier GetCurrentModifier()
    {
        if (IsMaxLevel())
            return null;

        return Modifiers[currentLevel];
    }
}
