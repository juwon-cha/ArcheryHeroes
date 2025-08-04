using UnityEngine;

public abstract class ConditionSO : ScriptableObject
{
    public abstract void Initialize();
    public abstract bool IsSatisfied();
}
