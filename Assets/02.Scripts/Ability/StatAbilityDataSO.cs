using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ability/StatAbilityData")]
public class StatAbilityDataSO : AbilityDataSO
{
    public new int MaxLevel => modifiers != null ? modifiers.Max(m => m.valuesPerLevel.Count) : 0;
    public List<StatModifier> modifiers;
}
