using System.Collections.Generic;

public enum StatModifierType
{
    AttackSpeed,
    MoveSpeed,
    Damage,
    Cooldown,
    MaxHP,
    CurrentHP,
    ExpGainRate, // 경험치 획득량 증가율 (예: +20%라면 1.2배 경험치)
    BackArrow, // 뒤로 화살 발사 (예: 1일 때 1개)
    Lightning, // 번개 효과
    Bounce // 튕김 효과 (예: 1일 때 1회 튕김, 2일 때 2회 튕김)
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