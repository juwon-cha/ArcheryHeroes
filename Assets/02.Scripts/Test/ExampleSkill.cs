using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/ExampleSkill")]
public class ExampleSkill : SkillBase
{
    public override void Activate(GameObject player)
    {
        if (CanUse)
        {
            // 스킬 사용 로직
            Debug.Log($"{skillName} activated by {player.name}");
            SetLastUsedTime(Time.time);
        }
        else
        {
            Debug.Log($"{skillName} is on cooldown.");
        }
    }
}
