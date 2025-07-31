using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ability/SkillAbilityData")]
public class SkillAbilityDataSO : AbilityDataSO
{
    public new int MaxLevel => skillDataSO.MaxLevel;
    public SkillDataSO skillDataSO;
}
