using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : Singleton<AbilityManager>
{
    public Action<AbilityData> OnAbilityLevelUp;

    [SerializeField] private AbilityDataSO[] allAbilities;

    private Dictionary<AbilityDataSO, AbilityData> abilityDataDict = new();
    private AbilityRandomSelector selector;
    private AbilityApplier abilityApplier;

    protected override void Initialize()
    {
        base.Initialize();

        abilityDataDict = new();
        foreach (var abilitySO in allAbilities)
            abilityDataDict[abilitySO] = new(abilitySO);

        selector = new(abilityDataDict.Values);
        abilityApplier = new();
    }

    public void ResetAbilities()
    {
        foreach (var abilitySO in allAbilities)
            abilityDataDict[abilitySO].Initialize();
    }

    // 랜덤으로 스킬 데이터들 가져오기
    public List<AbilityData> GetRandomAbilities(int count)
    {
        return selector.SelectRandomAbilities(count);
    }

    // 특정 스킬 데이터 가져오기
    public AbilityData GetAbilityData(AbilityDataSO abilitySO)
    {
        if (abilitySO == null)
        {
            Debug.LogError("SkillSO is null.");
            return null;
        }

        if (abilityDataDict.TryGetValue(abilitySO, out var skillData))
        {
            return skillData;
        }
        else
        {
            Debug.LogError($"SkillData for {abilitySO.abilityName} not found.");
            return null;
        }
    }



    // 특정 스킬 레벨업
    public void LevelUpSkill(AbilityDataSO abilitySO)
    {
        if(abilitySO == null )
        {
            Debug.LogError("SkillSO is null.");
            return;
        }

        if (abilityDataDict.TryGetValue(abilitySO, out var abilityData))
        {
            abilityData.LevelUp();
            abilityApplier.ApplyAbility(abilityData);
            OnAbilityLevelUp?.Invoke(abilityData);
            Debug.Log($"{abilitySO.abilityName} 레벨업: {abilityData.currentLevel}");
        }
    }
}
