using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityApplier
{
    public void ApplyAbility(AbilityData data)
    {
        if (data.abilitySO is StatAbilityDataSO statAbilityData)
        {
            // player.ApplyStatModifier(data.GetCurrentModifier());

        }
        else if (data.abilitySO is SkillAbilityDataSO skillAbilityData)
        {
            SkillManager.Instance.AddSkill(skillAbilityData.skillDataSO);
        }
    }
}
