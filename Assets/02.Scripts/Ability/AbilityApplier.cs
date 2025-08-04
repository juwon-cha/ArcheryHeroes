using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityApplier
{
    public void ApplyAbility(AbilityData data)
    {
        if (data.abilitySO is StatAbilityDataSO statAbilityData)
        {
            foreach (var statModifier in statAbilityData.modifiers)
                ApplyStat(statModifier, data);

            // player.ApplyStatModifier(data.GetCurrentModifier());
        }
        else if (data.abilitySO is SkillAbilityDataSO skillAbilityData)
        {
            SkillManager.Instance.AddSkill(skillAbilityData.skillDataSO);
        }
    }

    void ApplyStat(StatModifier statModifier, AbilityData data)
    {
        GameManager gameManager = GameManager.Instance;
        PlayerController player = gameManager.Player.GetComponent<PlayerController>();
        ResourceController resourceController = player.GetComponent<ResourceController>();
        RangedWeaponHandler rangedWeaponHandler = player.GetComponentInChildren<RangedWeaponHandler>(true);

        // 레벨업이 활성화된 경우, 현재 레벨에 맞는 값을 가져오고, 그렇지 않으면 레벨 1의 값을 가져옵니다.
        float value = data.abilitySO.isLevelUpEnabled? statModifier.GetValueByLevel(data.currentLevel) : statModifier.GetValueByLevel(1);

        switch (statModifier.type)
        {
            case StatModifierType.AttackSpeed:
                break;
            case StatModifierType.MoveSpeed:
                break;
            case StatModifierType.Damage:
                break;
            case StatModifierType.Cooldown:
                break;
            case StatModifierType.MaxHP:
                break;
            case StatModifierType.CurrentHP:
                resourceController.Heal(value);
                break;
            case StatModifierType.ExpGainRate:
                break;
            case StatModifierType.BackArrow:
                rangedWeaponHandler.numberOfProjectilesPerShot_Back = (int)value;
                break;
            case StatModifierType.Lightning:
                rangedWeaponHandler.ElementType = ElementType.Lightning;
                break;
            case StatModifierType.Bounce:
                rangedWeaponHandler.bounceCount = (int)value;
                break;

        }
    }
}
