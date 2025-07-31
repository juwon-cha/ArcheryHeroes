using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Effect/DebugEffect")]
public class DebugEffect : EffectSO
{
    [SerializeField] private string debugMessage = "Debug Effect Triggered";

    public override void Initialize()
    {
        Debug.Log($"Init {name}");
    }
    public override void Execute(EffectContext effectContext = null)
    {
        Debug.Log(debugMessage);
    }
}
