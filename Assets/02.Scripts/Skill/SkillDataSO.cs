using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public int MaxLevel => modifiers != null ? modifiers.Max(m => m.valuesPerLevel.Count) : 0;

    public List<StatModifier> modifiers;
}
