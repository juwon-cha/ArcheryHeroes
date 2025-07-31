using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract void Initialize();
    public abstract void Execute(EffectContext effectContext = null);
}
