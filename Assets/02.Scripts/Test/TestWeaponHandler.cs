using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponHandler : WeaponHandler
{
    public SkillBase skill;

    public override void Attack()
    {
        base.Attack();

        skill.Use(gameObject);
    }



}
