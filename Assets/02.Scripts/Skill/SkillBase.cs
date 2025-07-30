using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public string skillName; // 스킬 이름
    public float cooldown; // 쿨타임 (초 단위)
    public bool ignoreCooldown; // 쿨타임 없이 사용 가능한지 여부
    protected float lastUsedTime; // 마지막 사용 시간

    public virtual bool CanUse => Time.time >= lastUsedTime + cooldown;

    public virtual void Initialize() { }
    public void Use(GameObject player){
        if (!ignoreCooldown && !CanUse) return;
        OnUse(player); // 스킬 사용 로직 호출
        SetLastUsedTime(Time.time);
    }

    protected abstract void OnUse(GameObject player);

    public virtual void SetLastUsedTime(float time) => lastUsedTime = time;
    public void ResetCooldown() => lastUsedTime = 0f; // 쿨타임 초기화
}
