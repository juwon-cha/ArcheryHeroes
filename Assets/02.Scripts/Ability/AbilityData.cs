using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    public AbilityDataSO abilitySO;  // ������ �Ǵ� SO
    public int currentLevel;     // ���� ����

    public string SkillName { get => abilitySO.skillName; }
    public string Description { get => abilitySO.description; }
    public Sprite Icon { get => abilitySO.icon; }
    public int MaxLevel { get => abilitySO.MaxLevel; }
    public List<StatModifier> Modifiers { get => abilitySO.modifiers; }

    public AbilityData(AbilityDataSO skillSO)
    {
        this.abilitySO = skillSO;
        currentLevel = 0;
    }

    public bool IsMaxLevel()
    {
        return currentLevel >= abilitySO.MaxLevel;
    }

    public void LevelUp()
    {
        if (!abilitySO.isLevelUpEnabled) return;

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
