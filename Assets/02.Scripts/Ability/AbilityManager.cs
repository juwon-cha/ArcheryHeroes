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

    protected override void Initialize()
    {
        base.Initialize();

        abilityDataDict = new();
        foreach (var skillSO in allAbilities)
            abilityDataDict[skillSO] = new(skillSO);

        selector = new(abilityDataDict.Values);
    }

    // �������� ��ų �����͵� ��������
    public List<AbilityData> GetRandomAbilities(int count)
    {
        return selector.SelectRandomAbilities(count);
    }

    // Ư�� ��ų ������ ��������
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
            Debug.LogError($"SkillData for {abilitySO.skillName} not found.");
            return null;
        }
    }



    // Ư�� ��ų ������
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
            OnAbilityLevelUp?.Invoke(abilityData);
            Debug.Log($"{abilitySO.skillName} ������: {abilityData.currentLevel}");
        }
    }
}
