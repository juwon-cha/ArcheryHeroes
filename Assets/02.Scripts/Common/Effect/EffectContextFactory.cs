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

        if(effectSO is SpawnEffect effect)
        {
            pos = GameManager.Instance.Player.transform.position;
            caster = GameManager.Instance.Player.gameObject;
            target = GameManager.Instance.Player.gameObject;
        }

        return new(pos, caster, target);
    }
}
