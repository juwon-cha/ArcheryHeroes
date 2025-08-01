using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract void Initialize();
    public abstract void Execute(EffectContext effectContext = null);

    public virtual void Deactivate() { } // 레벨업해서 더이상 안쓸 때 사용
}
