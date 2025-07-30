using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public string skillName; // ��ų �̸�
    public float cooldown; // ��Ÿ�� (�� ����)
    protected float lastUsedTime; // ������ ��� �ð�

    public virtual bool CanUse => Time.time >= lastUsedTime + cooldown;

    public abstract void Activate(GameObject player);
    public void SetLastUsedTime(float time) => lastUsedTime = time;
    public void ResetCooldown() => lastUsedTime = 0f; // ��Ÿ�� �ʱ�ȭ
}
