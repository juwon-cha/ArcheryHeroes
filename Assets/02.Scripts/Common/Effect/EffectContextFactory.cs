using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectContextFactory
{
    public static EffectContext CreateEffectContext(EffectSO effectSO)
    {
        if (effectSO == null) return null;
        Vector2 pos = Vector2.zero;
        GameObject caster = null;
        GameObject target = null;

        GameManager gameManager = GameManager.Instance;

        if(effectSO is SpawnEffect)
        {
            pos = gameManager.Player.transform.position;
            caster = gameManager.Player.gameObject;
            target = gameManager.Player.gameObject;
        }
        else if (effectSO is TimedTeleport)
        {
            pos = gameManager.Player.transform.position;
            caster = gameManager.Player.gameObject;
            target = gameManager.Player.gameObject;
        }

        return new(pos, caster, target);
    }
}
