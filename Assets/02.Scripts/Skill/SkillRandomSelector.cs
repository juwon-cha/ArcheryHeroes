using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillRandomSelector
{
    private List<SkillDataSO> skillPool;
    private HashSet<SkillDataSO> excludedSkills = new();

    public SkillRandomSelector(IEnumerable<SkillDataSO> skills)
    {
        skillPool = new List<SkillDataSO>(skills);
    }

    public void ExcludeSkills(IEnumerable<SkillDataSO> skills)
    {
        excludedSkills = new HashSet<SkillDataSO>(skills);
    }

    // 중복 제외 후 최대 count 개수만큼 랜덤 스킬 반환
    public List<SkillDataSO> SelectRandomSkills(int count)
    {
        var available = skillPool.Where(s => !excludedSkills.Contains(s)).ToList();

        if (available.Count == 0)
            return new List<SkillDataSO>();

        count = Mathf.Min(count, available.Count);

        // 리스트 셔플
        for (int i = 0; i < available.Count; i++)
        {
            int j = Random.Range(i, available.Count);
            var temp = available[i];
            available[i] = available[j];
            available[j] = temp;
        }

        return available.GetRange(0, count);
    }
}
