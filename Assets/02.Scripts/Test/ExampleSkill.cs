using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/ExampleSkill")]
public class ExampleSkill : SkillBase
{
    protected override void OnUse(GameObject player)
    {
        SetLastUsedTime(Time.time);
    }
}
