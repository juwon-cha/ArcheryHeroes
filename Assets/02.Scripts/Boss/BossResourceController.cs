using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossResourceController : ResourceController
{
    private BossController bossController;

    protected override void Awake()
    {
        base.Awake();
        bossController = GetComponent<BossController>();
    }

    protected override void Death()
    {
        bossController.OnDead();
    }
}
