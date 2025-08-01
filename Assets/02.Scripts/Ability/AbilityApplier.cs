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
        RangedWeaponHandler rangedWeaponHandler = player.GetComponentInChildren<RangedWeaponHandler>(true);

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
                break;
            case StatModifierType.ExpGainRate:
                break;
            case StatModifierType.BackArrow:
                rangedWeaponHandler.numberOfProjectilesPerShot_Back = (int)statModifier.GetValueByLevel(data.currentLevel);
                break;
            case StatModifierType.Lightning:
                rangedWeaponHandler.ElementType = ElementType.Lightning;
                break;
            case StatModifierType.Bounce:
                rangedWeaponHandler.bounceCount = (int)statModifier.GetValueByLevel(data.currentLevel);
                break;

        }
    }
}
