using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillRandomSelector
{
    private List<SkillData> skillDatas;

    public SkillRandomSelector(IEnumerable<SkillData> skills)
    {
        skillDatas = new(skills);
    }

    // �ִ� ������ �ƴ� ��ų �߿��� ���ϴ� ������ŭ ���� ����
    public List<SkillData> SelectRandomSkills(int count)
    {
        var available = skillDatas.Where(s => !s.IsMaxLevel()).ToList();

        if (available.Count == 0)
            return null;

        count = Mathf.Min(count, available.Count);

        // ����Ʈ ���� (Fisher-Yates)
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
