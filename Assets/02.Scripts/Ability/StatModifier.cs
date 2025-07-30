using System.Collections.Generic;

public enum StatModifierType
{
    AttackSpeed,
    MoveSpeed,
    Damage,
    Cooldown,
    MaxHP,
    CurrentHP
}


[System.Serializable]
public class StatModifier
{
    public StatModifierType type;
    public bool isPercentage;

    public List<float> valuesPerLevel; // 예: [0.1f, 0.2f, 0.3f] → 1~3레벨일 때 증가량
    public float GetValueByLevel(int level)
    {
        if (level < 1 || level > valuesPerLevel.Count)
            return 0f;

        return valuesPerLevel[level - 1];
    }
}