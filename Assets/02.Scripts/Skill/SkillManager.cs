using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] private SkillDataSO[] allSkills;

    private Dictionary<SkillDataSO, SkillData> skillDataDict = new();
    private SkillRandomSelector selector;

    protected override void Initialize()
    {
        base.Initialize();

        foreach (var skillSO in allSkills)
            skillDataDict[skillSO] = new SkillData(skillSO);

        selector = new SkillRandomSelector(skillDataDict.Values);
    }

    // �������� ��ų �����͵� ��������
    public List<SkillData> GetRandomSkills(int count)
    {
        return selector.SelectRandomSkills(count);
    }

    // Ư�� ��ų ������
    public void LevelUpSkill(SkillDataSO skillSO)
    {
        if (skillDataDict.TryGetValue(skillSO, out var skillData))
        {
            skillData.LevelUp();
            Debug.Log($"{skillSO.skillName} ������: {skillData.currentLevel}");
        }
    }
}
