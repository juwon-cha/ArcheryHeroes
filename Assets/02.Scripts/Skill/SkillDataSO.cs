using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public bool isLevelUpEnabled = true; // 레벨업을 사용하는지 여부
    public int MaxLevel => modifiers != null ? modifiers.Max(m => m.valuesPerLevel.Count) : 0;

    public List<StatModifier> modifiers;
}
