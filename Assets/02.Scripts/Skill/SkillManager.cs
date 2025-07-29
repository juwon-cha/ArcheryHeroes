using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillDataSO[] allSkills;
    private SkillRandomSelector selector;
    private HashSet<SkillDataSO> acquiredSkills = new();

    private int playerLevel = 1;

    private void Awake()
    {
        selector = new SkillRandomSelector(allSkills);
    }

    public SkillDataSO GetRandomNewSkill()
    {
        selector.ExcludeSkills(acquiredSkills);
        return selector.SelectRandomSkill(playerLevel);
    }

    public void AcquireSkill(SkillDataSO skill)
    {
        if (skill == null || acquiredSkills.Contains(skill))
            return;

        acquiredSkills.Add(skill);
        Debug.Log($"»πµÊ«— Ω∫≈≥: {skill.skillName}");
    }
}
