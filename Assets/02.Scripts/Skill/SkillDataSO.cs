using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public string skillName; // 스킬 이름
    public bool ignoreCooldown; // 쿨타임 없이 사용 가능한지 여부

    [SerializeField] private List<SkillData> skillDatas; // 스킬 데이터

    public int MaxLevel => skillDatas.Count; // 최대 레벨 (0부터 시작하므로 -1)
    private int currentLevel; // 현재 레벨
    protected float lastUsedTime; // 마지막 사용 시간
    private EffectContext effectContext;

    public SkillData CurrentSkillData => skillDatas[currentLevel];

    public float Cooldown => CurrentSkillData.cooldown;

    public bool CanUse
    {
        get
        {
            bool isCooldownOver = Time.time >= lastUsedTime + Cooldown;
            bool areConditionsSatisfied = true;
            var conditions = CurrentSkillData.conditions;

            foreach (var condition in conditions)
            {
                if (!condition.IsSatisfied())
                {
                    areConditionsSatisfied = false;
                    break;
                }
            }
            return isCooldownOver && areConditionsSatisfied;
        }
    }

    public virtual void Initialize()
    {
        ResetCooldown();

        currentLevel = 0; // 스킬 초기화 시 레벨을 0으로 설정
        foreach (var skillData in skillDatas)
            skillData.Initialize(); // 각 스킬 데이터 초기화

    }
    public void Use(EffectContext effectContext = null){
        if (!ignoreCooldown && !CanUse) return;
        OnUse(effectContext); // 스킬 사용 로직 호출
        SetLastUsedTime(Time.time);
    }

    protected virtual void OnUse(EffectContext effectContext = null)
    {
        foreach (var effect in CurrentSkillData.effects)
        {
            effectContext = effectContext ?? EffectContextFactory.CreateEffectContext(effect); // context가 없으면 효과 컨텍스트 생성
            effect.Execute(effectContext); // 스킬 효과 적용
        }
    }

    public void LevelUp()
    {
        if (currentLevel >= MaxLevel) return;

        currentLevel++;
        ResetCooldown();
    }

    public virtual void SetLastUsedTime(float time) => lastUsedTime = time;
    public void ResetCooldown() => lastUsedTime = 0f; // 쿨타임 초기화
}
