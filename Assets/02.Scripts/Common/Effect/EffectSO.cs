using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract void Initialize(); // 이펙트가 처음 생성될 때 호출되는 함수
    public abstract void Execute(EffectContext effectContext = null); // 이펙트가 실행될 때 호출되는 함수
    public virtual void Deactivate() { } // 레벨업해서 더이상 안쓸 때 사용
}
