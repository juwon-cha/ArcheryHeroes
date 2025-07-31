using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    public AbilityDataSO abilitySO;  // 기준이 되는 SO
    public int currentLevel;     // 현재 레벨

    public string AbilityName { get => abilitySO.abilityName; }
    public string Description { get => abilitySO.description; }
    public Sprite Icon { get => abilitySO.icon; }
    public int MaxLevel { get => abilitySO.MaxLevel; }

    public AbilityData(AbilityDataSO abilitySO)
    {
        this.abilitySO = abilitySO;
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
}
