using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/StatModSkill")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;

    public List<StatModifier> modifiers;
}
