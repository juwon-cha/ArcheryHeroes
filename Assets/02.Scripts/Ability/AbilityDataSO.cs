using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbilityDataSO : ScriptableObject
{
    public string abilityName;
    public string description;
    public Sprite icon;
    public bool isLevelUpEnabled = true; // 레벨업을 사용하는지 여부
    public int MaxLevel
    {
        get
        {
            if (this is StatAbilityDataSO statAbilityData)
                return statAbilityData.MaxLevel;
            else if (this is SkillAbilityDataSO skillAbilityData)
                return skillAbilityData.MaxLevel;

            return 0;
        }
    }

}
