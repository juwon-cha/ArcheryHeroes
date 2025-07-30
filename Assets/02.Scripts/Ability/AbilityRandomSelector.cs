using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityRandomSelector
{
    private List<AbilityData> abilityDatas;

    public AbilityRandomSelector(IEnumerable<AbilityData> abilities)
    {
        abilityDatas = new(abilities);
    }

    // 최대 레벨이 아닌 스킬 중에서 원하는 개수만큼 랜덤 선택
    public List<AbilityData> SelectRandomAbilities(int count)
    {
        var available = abilityDatas.Where(s => !s.IsMaxLevel()).ToList();

        if (available.Count == 0)
            return null;

        count = Mathf.Min(count, available.Count);

        // 리스트 셔플 (Fisher-Yates)
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
