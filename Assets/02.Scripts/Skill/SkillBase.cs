using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public string skillName; // 스킬 이름
    public float cooldown; // 쿨타임 (초 단위)
    protected float lastUsedTime; // 마지막 사용 시간

    public virtual bool CanUse => Time.time >= lastUsedTime + cooldown;

    public abstract void Activate(GameObject player);
    public void SetLastUsedTime(float time) => lastUsedTime = time;
    public void ResetCooldown() => lastUsedTime = 0f; // 쿨타임 초기화
}
