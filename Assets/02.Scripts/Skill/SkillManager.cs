using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private HashSet<SkillDataSO> acquiredSkills; // 획득한 스킬들

    protected override void Initialize()
    {
        base.Initialize();

        acquiredSkills = new();
    }

    private void Update()
    {
        UseSkills();
    }

    public void UseSkills()
    {
        foreach (var skill in acquiredSkills)
        {
            if (skill == null) continue;

            skill.Use();
        }
    }

    public void AddSkill(SkillDataSO skill)
    {
        if(acquiredSkills.Contains(skill))
        {
            skill.LevelUp();
        }
        else
        {
            skill.Initialize();
            acquiredSkills.Add(skill);
        }
    }

    public void LevelUpSkill(SkillDataSO skill)
    {
        if (skill == null)
        {
            Debug.LogError("SkillDataSO is null.");
            return;
        }

        if (!acquiredSkills.Contains(skill))
        {
            Debug.LogError($"Skill {skill.skillName} not acquired yet.");
            return;
        }

        skill.LevelUp(); // 스킬 레벨업
    }

    public void RemoveSkill(SkillDataSO skill)
    {
        acquiredSkills.Remove(skill);
    }

}
