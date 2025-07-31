using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Effect/SpawnEffect")]
public class SpawnEffect : EffectSO
{
    public GameObject spawnObject;

    public override void Initialize() { }

    public override void Execute(EffectContext effectContext = null)
    {
        if (spawnObject == null || effectContext == null) return;

        ObjectPoolingManager.Instance.Get(spawnObject, effectContext.Position, Quaternion.identity);
    }
}
