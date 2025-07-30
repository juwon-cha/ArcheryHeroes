using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] private List<SkillBase> skills;
    private HashSet<SkillBase> acquiredSkills; // 획득한 스킬들

    protected override void Initialize()
    {
        base.Initialize();

        acquiredSkills = new HashSet<SkillBase>();

        foreach (var skill in skills)
            AddSkill(skill);
    }

    private void Update()
    {
        UseSkills(gameObject);
    }

    public void UseSkills(GameObject player)
    {
        foreach (var skill in acquiredSkills)
        {
            if (!skill.CanUse) continue;

            skill.Use(player);
            skill.SetLastUsedTime(Time.time);
        }
    }

    public void AddSkill(SkillBase skill)
    {
        skill.Initialize();
        acquiredSkills.Add(skill);
    }

    public void RemoveSkill(SkillBase skill)
    {
        acquiredSkills.Remove(skill);
    }

}
