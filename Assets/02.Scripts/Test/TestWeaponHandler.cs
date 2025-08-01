using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponHandler : WeaponHandler
{
    public SkillDataSO skill;

    public override void Attack()
    {
        base.Attack();

        EffectContext effectContext = new EffectContext(transform.position, gameObject);
        skill.Use(effectContext);
    }



}
