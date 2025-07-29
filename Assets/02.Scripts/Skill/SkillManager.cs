using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public Action<SkillData> OnSkillLevelUp;

    [SerializeField] private SkillDataSO[] allSkills;

    private Dictionary<SkillDataSO, SkillData> skillDataDict = new();
    private SkillRandomSelector selector;

    protected override void Initialize()
    {
        base.Initialize();

        skillDataDict = new();
        foreach (var skillSO in allSkills)
            skillDataDict[skillSO] = new SkillData(skillSO);

        selector = new SkillRandomSelector(skillDataDict.Values);
    }

    // �������� ��ų �����͵� ��������
    public List<SkillData> GetRandomSkills(int count)
    {
        Debug.Log($"Requesting {count} random skills.");
        return selector.SelectRandomSkills(count);
    }

    // Ư�� ��ų ������ ��������
    public SkillData GetSkillData(SkillDataSO skillSO)
    {
        if (skillSO == null)
        {
            Debug.LogError("SkillSO is null.");
            return null;
        }

        if (skillDataDict.TryGetValue(skillSO, out var skillData))
        {
            return skillData;
        }
        else
        {
            Debug.LogError($"SkillData for {skillSO.skillName} not found.");
            return null;
        }
    }



    // Ư�� ��ų ������
    public void LevelUpSkill(SkillDataSO skillSO)
    {
        if(skillSO == null )
        {
            Debug.LogError("SkillSO is null.");
            return;
        }

        if (skillDataDict.TryGetValue(skillSO, out var skillData))
        {
            skillData.LevelUp();
            OnSkillLevelUp?.Invoke(skillData);
            Debug.Log($"{skillSO.skillName} ������: {skillData.currentLevel}");
        }
    }
}
